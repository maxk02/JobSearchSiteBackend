using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Configurations;

public static class AuthConfigs
{
    public static void ConfigureJwtAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        if (configuration["Jwt:Key"] is null)
            throw new NotImplementedException();
        if (configuration["Jwt:Issuer"] is null)
            throw new NotImplementedException();
        if (configuration["Jwt:Audience"] is null)
            throw new NotImplementedException();
        if (configuration["Jwt:DurationDays"] is null)
            throw new NotImplementedException();
        var durationParsedSuccessfully = int.TryParse(configuration["Jwt:DurationDays"], out int durationDays);
        if (!durationParsedSuccessfully)
            throw new NotImplementedException();
        
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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
    }
}