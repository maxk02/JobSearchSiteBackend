using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ResetPassword;

public class ResetPasswordHandler(IIdentityService identityService)
    : IRequestHandler<ResetPasswordRequest, Result>
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var passwordResetResult = await identityService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }
}