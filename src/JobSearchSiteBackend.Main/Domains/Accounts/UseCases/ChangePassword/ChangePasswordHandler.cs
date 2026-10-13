using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Services.Caching;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;

public class ChangePasswordHandler(ICurrentAccountService currentAccountService,
    IUserSessionCacheRepository sessionCache,
    UserManager<MyIdentityUser> userManager) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        var currentTokenId = currentAccountService.GetTokenIdentifierOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);

        await sessionCache.DeleteAllSessionsExceptCurrentAsync(currentUserId.ToString(), currentTokenId);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}