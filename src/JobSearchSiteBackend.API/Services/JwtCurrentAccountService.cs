using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Services.Auth;
using Core.Services.Auth.Exceptions;

namespace JobSearchSiteBackend.API.Services;

public class JwtCurrentAccountService(IHttpContextAccessor httpContextAccessor) : ICurrentAccountService
{
    public string GetTokenIdentifierOrThrow()
    {
        string? tokenIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (string.IsNullOrEmpty(tokenIdString))
            throw new CurrentAccountDataNotAvailableException();

        return tokenIdString;
    }

    public long? GetId()
    {
        string? userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(userIdString))
            return null;

        return long.TryParse(userIdString, out long userId) ? userId : null;
    }

    public long GetIdOrThrow()
    {
        var id = GetId();
        return id ?? throw new CurrentAccountDataNotAvailableException();
    }

    public List<string>? GetRoles()
    {
        var roles = httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value).ToList();

        return roles;
    }

    public List<string> GetRolesOrThrow()
    {
        var roles = GetRoles();
        return roles ?? throw new CurrentAccountDataNotAvailableException();
    }

    public bool IsLoggedIn()
    {
        return httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public void ThrowIfNotLoggedIn()
    {
        var isLoggedIn = IsLoggedIn();
        if (!isLoggedIn)
            throw new CurrentAccountDataNotAvailableException();
    }
}