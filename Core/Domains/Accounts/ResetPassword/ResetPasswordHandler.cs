using Core.Services.Auth.AccountStorage;
using Shared.Result;

namespace Core.Domains.Accounts.ResetPassword;

public class ResetPasswordHandler(IAccountService accountService)
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var passwordResetResult = await accountService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }
}