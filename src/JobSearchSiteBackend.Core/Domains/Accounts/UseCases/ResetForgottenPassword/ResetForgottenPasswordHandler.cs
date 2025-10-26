using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetForgottenPassword;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetPassword;

public class ResetForgottenPasswordHandler(UserManager<MyIdentityUser> userManager)
    : IRequestHandler<ResetForgottenPasswordRequest, Result>
{
    public async Task<Result> Handle(ResetForgottenPasswordRequest request, CancellationToken cancellationToken = default)
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