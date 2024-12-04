using System.Security.Claims;
using Domain._Shared.Services.Auth;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

public class JwtCurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtCurrentAccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetId()
    {
        string? userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return string.IsNullOrEmpty(userIdString) ? null : userIdString;
    }

    public List<string> GetRoles()
    {
        List<string> roles = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value).ToList() ?? [];
        
        return roles;
    }
}