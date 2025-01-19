using Amazon.S3;
using Core.Domains.Companies.Search;
using Core.Domains.Cvs.Search;
using Core.Domains.Jobs.Search;
using Core.Domains.Locations.Search;
using Core.Domains.PersonalFiles.Search;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Core.Services.FileStorage;
using Core.Services.TextExtraction;
using Elasticsearch.Net;
using Hangfire;
using Infrastructure.BackgroundJobs.Hangfire;
using Infrastructure.EmailSender.MailKit;
using Infrastructure.FileStorage.AmazonS3;
using Infrastructure.Search.Elasticsearch;
using Infrastructure.TextExtraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Infrastructure;

public static class ServiceExtensions
{
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
    
    public static void ConfigureTextExtraction(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITextExtractionService, TextExtractionService>();
    }
    
    public static void ConfigureHangfire(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHangfire(config =>
            config.UseSqlServerStorage(Environment.GetEnvironmentVariable("MAIN_DB_CONNECTION_STRING")));
        serviceCollection.AddHangfireServer(options =>
        {
            options.Queues = BackgroundJobQueues.AllValues;
        });
        serviceCollection.AddSingleton<IBackgroundJobService, HangfireBackgroundJobService>();
    }

    public static void ConfigureMailKit(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEmailSenderService, MailKitEmailSenderService>();
    }
}