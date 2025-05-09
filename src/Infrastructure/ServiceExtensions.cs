using System.Text;
using Amazon.S3;
using Core.Domains.Accounts;
using Core.Domains.Companies.Search;
using Core.Domains.Jobs.Search;
using Core.Domains.Locations.Search;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Core.Services.Caching;
using Core.Services.EmailSender;
using Core.Services.FileStorage;
using Core.Services.TextExtraction;
using Elasticsearch.Net;
using Hangfire;
using JobSearchSiteBackend.Infrastructure.Auth;
using JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire;
using JobSearchSiteBackend.Infrastructure.Caching;
using JobSearchSiteBackend.Infrastructure.EmailSender.MailKit;
using JobSearchSiteBackend.Infrastructure.FileStorage.AmazonS3;
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

namespace JobSearchSiteBackend.Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceWithIdentity(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<MainDataContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("MAIN_DB_CONNECTION_STRING")));
        
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
    }
    
    public static void ConfigureJwtAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var issuer = configuration.GetSection("Jwt:Issuer").Value;
        var audience = configuration.GetSection("Jwt:Audience").Value;
        var secretKey = configuration.GetSection("Jwt:SecretKey").Value;

        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey))
            throw new ArgumentNullException();
        
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Name = "auth_token";
            });
    }
    
    public static void ConfigureJwtTokenGeneration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJwtTokenGenerationService, JwtTokenGenerationService>();
    }
    
    public static void ConfigureFileStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var bucketName = configuration["AWS:BucketName"];
        
        if (string.IsNullOrEmpty(bucketName))
            throw new ArgumentNullException();
        
        serviceCollection.AddDefaultAWSOptions(configuration.GetAWSOptions());
        
        serviceCollection.AddAWSService<IAmazonS3>();
        
        serviceCollection.AddSingleton<IFileStorageService, AmazonS3FileStorageService>(provider => 
            new AmazonS3FileStorageService(provider.GetRequiredService<IAmazonS3>(), bucketName));
    }
    
    public static void ConfigureSearch(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IElasticClient>(provider =>
        {
            var uri = configuration["Elasticsearch:Uri"];
            var username = configuration["Elasticsearch:Username"];
            var password = configuration["Elasticsearch:Password"];
            
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();

            var node = new Uri(uri);
            var connectionPool = new SingleNodeConnectionPool(node);
            var connectionSettings = new ConnectionSettings(connectionPool)
                .BasicAuthentication(username, password);

            var client = new ElasticClient(connectionSettings);
            return client;
        });
        
        serviceCollection.AddSingleton<IJobSearchRepository, ElasticJobSearchRepository>();
        serviceCollection.AddSingleton<ILocationSearchRepository, ElasticLocationSearchRepository>();
        serviceCollection.AddSingleton<IPersonalFileSearchRepository, ElasticPersonalFileSearchRepository>();
    }
    
    public static void ConfigureTextExtraction(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITextExtractionService, TextExtractionService>();
    }
    
    public static void ConfigureBackgroundJobScheduler(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHangfire(config =>
            config.UseSqlServerStorage(Environment.GetEnvironmentVariable("MAIN_DB_CONNECTION_STRING")));
        serviceCollection.AddHangfireServer(options =>
        {
            options.Queues = BackgroundJobQueues.AllValues;
        });
        serviceCollection.AddSingleton<IBackgroundJobService, HangfireBackgroundJobService>();
    }

    public static void ConfigureEmailSender(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEmailSenderService, MailKitEmailSenderService>();
    }
    
    public static void ConfigureMemoryCache(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(ICache<,>), typeof(MemoryCache<,>));
    }
}