using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services.Auth.Authentication;

public class JwtGenerationService(IConfiguration configuration) : IJwtGenerationService
{
    public string Generate(AccountData accountData)
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
            new(JwtRegisteredClaimNames.Sub, accountData.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, accountData.Email)
        ];
        claims.AddRange(accountData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
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