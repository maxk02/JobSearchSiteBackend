using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetPassword;

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