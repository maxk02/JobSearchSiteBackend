using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    ICache<string, UserSession> sessionCache) : IRequestHandler<DeleteAccountRequest, Result>
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        var currentUserTokenId = currentAccountService.GetTokenIdentifierOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.Error();
        
        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var aspNetIdentityResult = await userManager.DeleteAsync(user);
        
        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();
        
        await sessionCache.RemoveAsync($"user_session_{currentUserTokenId}");
        
        var userAvatars = await context.UserAvatars
            .Where(ua => ua.UserId == currentUserId)
            .ToListAsync(cancellationToken);
        
        foreach (var userAvatar in userAvatars)
            userAvatar.IsDeleted = true;

        context.UserAvatars.UpdateRange(userAvatars);
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}