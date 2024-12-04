using Shared.Result;

namespace Domain._Shared.Services.Auth;

public interface ICurrentAccountService
{
    public Result<UserClaimsDto> GetUserClaims();
}