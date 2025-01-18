using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.ConfirmEmail;

public class ConfirmEmailHandler(UserManager<MyIdentityUser> userManager) 
    : IRequestHandler<ConfirmEmailRequest, Result>
{
    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(
                u => userManager
                    .VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, "EmailConfirmation", request.Token).Result,
                cancellationToken
            );
        
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ConfirmEmailAsync(user, request.Token);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}