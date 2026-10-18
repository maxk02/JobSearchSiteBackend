using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JobSearchSiteBackend.Main.Services.Auth;

public class JwtTokenGenerationService(IConfiguration configuration) : IJwtTokenGenerationService
{
    public string Generate(AccountData accountData, Guid newTokenId)
    {
        var issuer = configuration["JWT_ISSUER"];
        var audience = configuration["JWT_AUDIENCE"];
        var secretKey = configuration["JWT_SECRET_KEY"];

        if (string.IsNullOrEmpty(issuer)
            || string.IsNullOrEmpty(audience)
            || string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException();
        }
        
        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, accountData.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, newTokenId.ToString()),
            new("EmailConfirmed", accountData.EmailConfirmed.ToString().ToLower())
        ];
        claims.AddRange(accountData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            signingCredentials: credentials,
            notBefore: DateTime.UtcNow);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}