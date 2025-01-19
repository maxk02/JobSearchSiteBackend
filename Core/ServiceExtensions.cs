using System.Text;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.MyAppSettings;

namespace Core;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceWithIdentity(this IServiceCollection serviceCollection,
        IConfiguration configuration)
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
            });
    }
}