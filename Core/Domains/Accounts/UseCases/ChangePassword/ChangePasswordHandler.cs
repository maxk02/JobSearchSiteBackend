using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ChangePassword;

public class ChangePasswordHandler(IJwtCurrentAccountService jwtCurrentAccountService, 
    UserManager<MyIdentityUser> userManager) : IRequestHandler<ChangePasswordRequest, Result>
{
    public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = jwtCurrentAccountService.GetIdOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}