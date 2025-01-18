using Amazon.S3;
using Core.Domains.Companies.Search;
using Core.Domains.Cvs.Search;
using Core.Domains.Jobs.Search;
using Core.Domains.Locations.Search;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Core.Services.FileStorage;
using Elasticsearch.Net;
using Hangfire;
using Infrastructure.BackgroundJobs.Hangfire;
using Infrastructure.EmailSender.SendGrid;
using Infrastructure.FileStorage.AmazonS3;
using Infrastructure.Search.Elasticsearch;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceWithIdentity(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<MainDataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MainDb")));
        
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
    
    public static void ConfigureAmazonS3(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var bucketName = configuration["AWS:BucketName"];
        
        if (string.IsNullOrEmpty(bucketName))
            throw new ArgumentNullException();
        
        serviceCollection.AddDefaultAWSOptions(configuration.GetAWSOptions());
        
        serviceCollection.AddAWSService<IAmazonS3>();
        
        serviceCollection.AddSingleton<IFileStorageService, AmazonS3FileStorageService>(provider => 
            new AmazonS3FileStorageService(provider.GetRequiredService<IAmazonS3>(), bucketName));
    }
    
    public static void ConfigureElasticSearch(this IServiceCollection serviceCollection, IConfiguration configuration)
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

        serviceCollection.AddSingleton<ICompanySearchRepository, ElasticCompanySearchRepository>();
        serviceCollection.AddSingleton<ICvSearchRepository, ElasticCvSearchRepository>();
        serviceCollection.AddSingleton<IJobSearchRepository, ElasticJobSearchRepository>();
        serviceCollection.AddSingleton<ILocationSearchRepository, ElasticLocationSearchRepository>();
        serviceCollection.AddSingleton<IPersonalFileSearchRepository, ElasticPersonalFileSearchRepository>();
    }
    
    public static void ConfigureHangfire(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("MainDb")));
        serviceCollection.AddHangfireServer(options =>
        {
            options.Queues = BackgroundJobQueues.AllValues;
        });
        serviceCollection.AddSingleton<IBackgroundJobService, HangfireBackgroundJobService>();
    }

    public static void ConfigureSendGrid(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<IEmailSenderService, SendGridEmailSenderService>(provider =>
        {
            var sendGridApiKey = configuration["SendGrid:ApiKey"];
            var senderEmail = configuration["SendGrid:SenderEmail"];
            var domainName = configuration["DomainName"];

            if (string.IsNullOrEmpty(sendGridApiKey) 
                || string.IsNullOrEmpty(senderEmail)
                || string.IsNullOrEmpty(domainName))
            {
                throw new ArgumentNullException();
            }

            return new SendGridEmailSenderService(sendGridApiKey, senderEmail, domainName);
        });
    }
}