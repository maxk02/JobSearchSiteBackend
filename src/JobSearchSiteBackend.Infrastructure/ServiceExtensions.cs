using System.Reflection;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Elasticsearch.Net;
using Hangfire;
using Hangfire.SqlServer;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Companies.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Companies.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.Search;
using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.Locations.Search;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobRunners;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Core.Services.FileStorage;
using JobSearchSiteBackend.Core.Services.TextExtraction;
using JobSearchSiteBackend.Infrastructure.Auth;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.BackgroundJobsRunners;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;
using JobSearchSiteBackend.Infrastructure.Caching;
using JobSearchSiteBackend.Infrastructure.EmailSender.MailKit;
using JobSearchSiteBackend.Infrastructure.FileStorage.CloudflareR2;
using JobSearchSiteBackend.Infrastructure.Persistence.EfCore;
using JobSearchSiteBackend.Infrastructure.Persistence.SqlServer;
using JobSearchSiteBackend.Infrastructure.Search.Elasticsearch;
using JobSearchSiteBackend.Infrastructure.TextExtraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nest;
using StackExchange.Redis;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace JobSearchSiteBackend.Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceWithIdentity(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton<UpdateTimestampInterceptor>();
        
        serviceCollection.AddDbContext<MainDataContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<UpdateTimestampInterceptor>();
            options
                .UseSqlServer(configuration["SQL_SERVER_DEFAULT_CONNECTION_STRING"],
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly()))
                .AddInterceptors(interceptor)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        });
        
        serviceCollection.AddIdentity<MyIdentityUser, MyIdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<MainDataContext>()
            .AddDefaultTokenProviders();
        
        serviceCollection.AddSingleton<IInjectableSqlQueries, SqlServerInjectableSqlQueries>();

        serviceCollection.AddScoped<MainDataContextSeeder>();

        // This stops the 302 Redirects and returns 401/403 JSON-friendly errors
        serviceCollection.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });
    }
    
    public static void ConfigureJwtAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var issuer = configuration["JWT_ISSUER"];
        var audience = configuration["JWT_AUDIENCE"];
        var secretKey = configuration["JWT_SECRET_KEY"];

        if (string.IsNullOrEmpty(issuer)
            || string.IsNullOrEmpty(audience)
            || string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException();
        }
        
        serviceCollection
            .AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Look for the "auth_token" cookie
                        var token = context.Request.Cookies["auth_token"];
                        
                        // If found, pass it to the JWT validator
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Auth failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully.");
                        return Task.CompletedTask;
                    }
                };
            });
            // .AddCookie(options =>
            // {
            //     options.Cookie.HttpOnly = true;
            //     options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            //     options.Cookie.SameSite = SameSiteMode.Lax;
            //     options.Cookie.Name = "auth_token";
            // });
    }
    
    public static void ConfigureJwtTokenGeneration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJwtTokenGenerationService, JwtTokenGenerationService>();
    }
    
    // public static void ConfigureFileStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    // {
    //     serviceCollection.AddDefaultAWSOptions(configuration.GetAWSOptions());
        
    //     serviceCollection.AddAWSService<IAmazonS3>();
        
    //     serviceCollection.AddSingleton<IFileStorageService, AmazonS3FileStorageService>(provider => 
    //         new AmazonS3FileStorageService(provider.GetRequiredService<IAmazonS3>(), configuration));
    // }

    public static void ConfigureFileStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var r2AccountId = configuration["CLOUDFLARE_R2_ACCOUNT_ID"];
        var accessKey = configuration["CLOUDFLARE_R2_ACCESS_KEY_ID"];
        var secretKey = configuration["CLOUDFLARE_R2_SECRET_KEY"];

        if (string.IsNullOrWhiteSpace(r2AccountId) || 
            string.IsNullOrWhiteSpace(accessKey) || 
            string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentNullException("Cloudflare R2 configuration is missing.");
        }

        AWSConfigsS3.UseSignatureVersion4 = true;

        var r2Url = $"https://{r2AccountId}.r2.cloudflarestorage.com";

        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        var s3Config = new AmazonS3Config
        {
            ServiceURL = r2Url,
            SignatureVersion = "4"
        };

        // 3. Register the AmazonS3Client manually
        serviceCollection.AddSingleton<IAmazonS3>(sp => 
            new AmazonS3Client(credentials, s3Config));

        // 4. Register your new R2 Service
        serviceCollection.AddSingleton<IFileStorageService, CloudflareR2FileStorageService>();
    }
    
    public static void ConfigureSearch(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IElasticClient>(provider =>
        {
            var uri = configuration["ELASTICSEARCH_URI"];
            var username = configuration["ELASTICSEARCH_USERNAME"];
            var password = configuration["ELASTICSEARCH_PASSWORD"];

            if (string.IsNullOrEmpty(uri)
                || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            var node = new Uri(uri);
            var connectionPool = new SingleNodeConnectionPool(node);
            var connectionSettings = new ConnectionSettings(connectionPool);

            var client = new ElasticClient(connectionSettings);
            return client;
        });
        
        serviceCollection.AddSingleton<IJobSearchRepository, ElasticJobSearchRepository>();
        serviceCollection.AddSingleton<ILocationSearchRepository, ElasticLocationSearchRepository>();
        serviceCollection.AddSingleton<IJobApplicationSearchRepository, ElasticJobApplicationSearchRepository>();
    }
    
    public static void ConfigureTextExtraction(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITextExtractionService, TextExtractionService>();
    }
    
    public static void ConfigureBackgroundJobScheduler(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration["HANGFIRE_DEFAULT_SQL_SERVER_CONNECTION_STRING"];
        
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException();
        
        // serviceCollection.AddHangfire(config =>
        //     config.UseSqlServerStorage(connectionString));

        serviceCollection.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                PrepareSchemaIfNecessary = true 
            }));
        
        serviceCollection.AddHangfireServer(options =>
        {
            options.Queues = BackgroundJobQueues.AllValues;
        });

        serviceCollection.AddSingleton<IRecurringJobRegisterer, HangfireRecurringJobRegisterer>();

        serviceCollection.AddSingleton<IDeletePersonalFileFromStorageRunner, DeletePersonalFileFromStorageRunner>();
        serviceCollection.AddSingleton<IResendEmailConfirmationLinkRunner, ResendEmailConfirmationLinkRunner>();
        serviceCollection.AddSingleton<ISendAccountCreatedEmailRunner, SendAccountCreatedEmailRunner>();
        serviceCollection
            .AddSingleton<ISendApplicationStatusUpdatedEmailRunner, SendApplicationStatusUpdatedEmailRunner>();
        serviceCollection.AddSingleton<ISendPasswordResetLinkRunner, SendPasswordResetLinkRunner>();
        serviceCollection.AddSingleton<ISendCompanyEmployeeInvitationEmailRunner, SendCompanyEmployeeInvitationEmailRunner>();

        serviceCollection.AddScoped<IClearCompanyAvatarsRunner, ClearCompanyAvatarsRunner>();
        serviceCollection.AddScoped<IDeleteNonUploadedFilesRunner, DeleteNonUploadedFilesRunner>();
        serviceCollection.AddScoped<ISyncJobsWithSearchRunner, SyncJobsWithSearchRunner>();
        serviceCollection.AddScoped<ISyncJobApplicationsWithSearchRunner, SyncJobApplicationsWithSearchRunner>();
    }

    public static void ConfigureEmailSender(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEmailSenderService, MailKitEmailSenderService>();
    }
    
    public static void ConfigureMemoryCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var options = new ConfigurationOptions
        {
            // Use your Redis server endpoint and port
            EndPoints = { { configuration["REDIS_HOST"] ?? throw new NullReferenceException(),
                int.Parse(configuration["REDIS_PORT"] ?? throw new NullReferenceException()) } }, 
    
            // 2. Enable TLS/SSL for encrypted communication
            Ssl = false, 
    
            // Optionally specify the TLS version (recommended: Tls12 or Tls13)
            // SslProtocols = SslProtocols.Tls12, 
    
            // 3. Provide Authentication
            // Use the password set in your redis.conf (requirepass) 
            // or the password associated with an ACL user.
            Password = configuration["REDIS_PASSWORD"] ?? throw new NullReferenceException(), 
    
            // If using Redis 6+ ACLs, specify the username
            // User = configuration["REDIS_USER"] ?? throw new NullReferenceException(), 
    
            // Optional: Only one connection is generally needed, use multiplexing.
            AbortOnConnectFail = false 
        };
        
        // 1. Register IConnectionMultiplexer as a Singleton
        // This is the recommended approach: create a single connection and reuse it.
        serviceCollection.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options));

        // 2. Register your abstraction
        serviceCollection.AddScoped<IGeneralCacheRepository, RedisGeneralCacheRepository>();
        serviceCollection.AddScoped<IUserSessionCacheRepository, RedisUserSessionCacheRepository>();
        serviceCollection.AddScoped<IPageVisitCacheRepository, RedisPageVisitCacheRepository>();
        serviceCollection.AddScoped<ICompanyLastVisitedJobsCacheRepository, RedisCompanyLastVisitedJobsCacheRepository>();
    }
}