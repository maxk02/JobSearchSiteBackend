﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JobSearchSiteBackend.Shared.MyAppSettings;

namespace JobSearchSiteBackend.Infrastructure.Auth;

public class JwtTokenGenerationService(IOptions<MyJwtSettings> settings) : IJwtTokenGenerationService
{
    public string Generate(AccountData accountData, Guid newTokenId)
    {
        var secretKey= settings.Value.SecretKey;

        var issuer = settings.Value.Issuer;

        var audience= settings.Value.Audience;
        
        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Sub, accountData.Id.ToString()),
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