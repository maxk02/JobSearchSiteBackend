using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ResetPassword;

public class ResetPasswordHandler(UserManager<MyIdentityUser> userManager)
    : IRequestHandler<ResetPasswordRequest, Result>
{
    public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(
                u => userManager
                    .VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, "ResetPassword", request.Token).Result,
                cancellationToken
            );

        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}