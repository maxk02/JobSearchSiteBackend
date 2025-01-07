using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context) : IRequestHandler<DeleteAccountRequest, Result>
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return Result.Error();
        
        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var aspNetIdentityResult = await userManager.DeleteAsync(user);
        
        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}