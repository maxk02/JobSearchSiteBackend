using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(
    IJwtCurrentAccountService jwtCurrentAccountService,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context) : IRequestHandler<DeleteAccountRequest, Result>
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = jwtCurrentAccountService.GetIdOrThrow();
        var currentUserJwtId = jwtCurrentAccountService.GetTokenIdentifierOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.Error();
        
        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var aspNetIdentityResult = await userManager.DeleteAsync(user);
        
        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();
        
        var userToRemove = await context.UserProfiles.FindAsync([currentUserId], CancellationToken.None);
        if (userToRemove is not null)
            context.UserProfiles.Remove(userToRemove);

        var newBlacklistedJwt = new BlacklistedJwt(currentUserJwtId);
        
        context.BlacklistedJwts.Add(newBlacklistedJwt);
        
        await context.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}