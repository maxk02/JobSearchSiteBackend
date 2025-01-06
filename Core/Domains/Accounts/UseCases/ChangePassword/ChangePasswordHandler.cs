using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ChangePassword;

public class ChangePasswordHandler(ICurrentAccountService currentAccountService, 
    IIdentityService identityService) : IRequestHandler<ChangePasswordRequest, Result>
{
    public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var changePasswordResult = await identityService
            .ChangePasswordAsync(currentUserId, request.OldPassword, request.NewPassword, cancellationToken);
        
        return changePasswordResult;
    }
}