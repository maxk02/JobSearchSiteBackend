using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Services.Auth.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth.Authentication;

public class JwtCurrentAccountService(IHttpContextAccessor httpContextAccessor) : ICurrentAccountService
{
    public long? GetId()
    {
        string? userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(userIdString))
            return null;

        return long.TryParse(userIdString, out long userId) ? userId : null;
    }

    public string? GetEmail()
    {
        string? emailString = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Email);

        return string.IsNullOrEmpty(emailString) ? null : emailString;
    }

    public List<string>? GetRoles()
    {
        var roles = httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value).ToList();

        return roles;
    }
}