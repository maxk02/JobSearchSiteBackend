using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.ChangePassword;

public class ChangePasswordHandler(IAccountService accountService, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId is null)
            return Result.Unauthorized();
        
        var changePasswordResult = await accountService
            .ChangePasswordAsync(currentUserId.Value, request.OldPassword, request.NewPassword, cancellationToken);
        
        return changePasswordResult;
    }
}