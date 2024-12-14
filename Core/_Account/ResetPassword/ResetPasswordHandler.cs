using Core._Shared.Services.Auth;
using Shared.Result;

namespace Core._Account.ResetPassword;

public class ResetPasswordHandler(IAccountService accountService)
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var passwordResetResult = await accountService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }
}