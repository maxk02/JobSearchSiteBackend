using Shared.Result;

namespace Application.Services.Auth;

public interface ICurrentAccountService
{
    public Result<UserClaimsDto> GetUserClaims();
}