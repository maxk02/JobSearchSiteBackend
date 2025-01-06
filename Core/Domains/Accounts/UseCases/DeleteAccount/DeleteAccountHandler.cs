using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.UserProfiles;
using Core.Persistence.EfCore;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Core.Services.BackgroundJobs;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(
    ICurrentAccountService currentAccountService,
    IIdentityService identityService,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<DeleteAccountRequest, Result>
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetIdOrThrow() != request.Id)
            return Result.Forbidden();
        
        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var deletionResult = await identityService.DeleteAsync(request.Id, cancellationToken);
        
        var userToRemove = await context.UserProfiles.FindAsync([request.Id], CancellationToken.None);
        if (userToRemove is not null)
            context.UserProfiles.Remove(userToRemove);

        var newBlacklistedJwt = new BlacklistedJwt(request.Token);
        
        context.BlacklistedJwts.Add(newBlacklistedJwt);
        
        await context.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}