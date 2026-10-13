using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;

public class ConfirmEmailHandler(UserManager<MyIdentityUser> userManager) 
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(
                u => userManager
                    .VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, "EmailConfirmation", command.Token).Result,
                cancellationToken
            );
        
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ConfirmEmailAsync(user, command.Token);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}