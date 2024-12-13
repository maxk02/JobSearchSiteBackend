using API.Services.Auth.AccountStorage;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.ResetPassword;

public class ResetPasswordHandler(IAccountStorageService accountStorageService)
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var passwordResetResult = await accountStorageService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }
}