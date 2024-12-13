using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services.Auth.CurrentAccount;

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