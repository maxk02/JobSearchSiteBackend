using System.Security.Claims;
using Domain._Shared.Services.Auth;
using Microsoft.AspNetCore.Http;
using Shared.Result;

namespace Infrastructure.Auth;

public class JwtCurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtCurrentAccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Result<UserClaimsDto> GetUserClaims()
    {
        if (_httpContextAccessor.HttpContext is null)
            return Result.Error();

        string? userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userIdString is null)
            return Result.Unauthorized();

        List<string>? roles = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value).ToList();

        if (roles is null)
            return Result.Error();

        return new UserClaimsDto(userIdString, roles);
    }
}