using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain._Shared.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public class JwtTokenGenerationService(IConfiguration configuration) : ITokenGenerationService
{
    public string Generate(UserClaimsDto userClaims)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        var secretKey = jwtSettings["Secret"];
        if (secretKey is null)
            throw new NotImplementedException();

        var issuer = jwtSettings["Issuer"];
        if (issuer is null)
            throw new NotImplementedException();

        var audience = jwtSettings["Audience"];
        if (audience is null)
            throw new NotImplementedException();
        
        var expirationMinutesParsed = int.TryParse(jwtSettings["ExpirationMinutes"], out int expirationMinutes);
        if (!expirationMinutesParsed)
            throw new NotImplementedException();
        
        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Sub, userClaims.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
        claims.AddRange(userClaims.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience : audience,
            claims : claims,
            expires: DateTime.Now.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}