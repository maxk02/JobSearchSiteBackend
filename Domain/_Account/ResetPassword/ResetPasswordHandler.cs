using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain._Account.ResetPassword;

public class ResetPasswordHandler(IAccountService accountService)
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var passwordResetResult = await accountService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }
}