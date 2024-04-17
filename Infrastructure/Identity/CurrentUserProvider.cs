using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Providers;

namespace Infrastructure.Identity;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int? UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isParsedSuccessfully = int.TryParse(userIdString, out int userIdToReturn);
            return isParsedSuccessfully ? userIdToReturn : null;
        }
    }
}