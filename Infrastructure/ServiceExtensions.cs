using System.Text;
using Application.Common.Exceptions;
using Application.Services.Email;
using Infrastructure.Auth.AspNetCoreIdentity;
using Infrastructure.Email;
using Infrastructure.Email.SendGrid;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigureDataPersistence(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<MyEfCoreDataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataDbConnection")));
    }
    
    public static void ConfigureAuthenticationPersistence(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<MyIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));

        serviceCollection.AddIdentity<MyIdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<MyIdentityDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureEmailSenderService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<IEmailSenderService, SendGridEmailSenderService>(provider =>
        {
            var sendGridApiKey = configuration["SendGrid:ApiKey"];
            var senderEmail = configuration["SendGrid:SenderEmail"];
            if (sendGridApiKey == null || senderEmail == null)
                throw new AppSettingsNullException();

            return new SendGridEmailSenderService(sendGridApiKey, senderEmail);
        });
    }
}