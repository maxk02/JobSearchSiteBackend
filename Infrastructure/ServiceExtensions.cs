using System.Text;
using Application.Common.Exceptions;
using Application.Services.Email;
using Infrastructure.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Infrastructure.Identity;
using Infrastructure.Persistence;

namespace Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigureInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        if (configuration["Jwt:Key"] is null)
            throw new AppSettingsNullException();
        if (configuration["Jwt:Issuer"] is null)
            throw new AppSettingsNullException();
        if (configuration["Jwt:Audience"] is null)
            throw new AppSettingsNullException();
        if (configuration["Jwt:DurationDays"] is null)
            throw new AppSettingsNullException();
        var durationParsedSuccessfully = int.TryParse(configuration["Jwt:DurationDays"], out int durationDays);
        if (!durationParsedSuccessfully)
            throw new AppSettingsBadValueException();
        
        
        serviceCollection.AddDbContext<DataDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataDbConnection")));
        
        serviceCollection.AddDbContext<MyIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));

        serviceCollection.AddIdentity<MyIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MyIdentityDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.SignIn.RequireConfirmedEmail = true;
        });

        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });
        
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