using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public class JwtTokenGenerationService : ITokenGenerationService
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string? Generate(long userId, ICollection<string> roles)
    {
        var secretKeyString = _configuration["Jwt:Key"];
        if (secretKeyString is null)
            throw new NotImplementedException();

        var issuerString = _configuration["Jwt:Issuer"];
        if (issuerString is null)
            throw new NotImplementedException();

        var audienceString = _configuration["Jwt:Audience"];
        if (audienceString is null)
            throw new NotImplementedException();
        
        if (_configuration["Jwt:DurationDays"] is null)
            throw new NotImplementedException();
        
        var durationParsedSuccessfully = int.TryParse(_configuration["Jwt:DurationDays"], out int durationDays);
        if (!durationParsedSuccessfully)
            throw new NotImplementedException();
        
        List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, userId.ToString())];
        //
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        //
        
        var userKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
        var credentials = new SigningCredentials(userKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuerString,
            audienceString,
            claims,
            expires: DateTime.Now.AddDays(durationDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}