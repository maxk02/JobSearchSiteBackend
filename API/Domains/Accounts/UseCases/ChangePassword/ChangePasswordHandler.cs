using API.Services.Auth.AccountStorage;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.ChangePassword;

public class ChangePasswordHandler(IAccountStorageService accountStorageService, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId is null)
            return Result.Unauthorized();
        
        var changePasswordResult = await accountStorageService
            .ChangePasswordAsync(currentUserId.Value, request.OldPassword, request.NewPassword, cancellationToken);
        
        return changePasswordResult;
    }
}