using Amazon.S3;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.EmailSender;
using Hangfire;
using Infrastructure.EmailSender.SendGrid;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<MainDataContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void ConfigureAmazonS3(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDefaultAWSOptions(configuration.GetAWSOptions());
        serviceCollection.AddAWSService<IAmazonS3>();
    }
    
    public static void ConfigureHangfire(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("HangfireDb")));
        serviceCollection.AddHangfireServer();
    }

    public static void ConfigureSendGrid(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<IEmailSenderService, SendGridEmailSenderService>(provider =>
        {
            var sendGridApiKey = configuration["SendGrid:ApiKey"];
            var senderEmail = configuration["SendGrid:SenderEmail"];
            if (sendGridApiKey == null || senderEmail == null)
                throw new Exception();

            return new SendGridEmailSenderService(sendGridApiKey, senderEmail);
        });
    }
}