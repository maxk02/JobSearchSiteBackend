using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.ChangePassword;

public class ChangePasswordHandler(ICurrentAccountService currentAccountService, 
    UserManager<MyIdentityUser> userManager) : IRequestHandler<ChangePasswordRequest, Result>
{
    public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}