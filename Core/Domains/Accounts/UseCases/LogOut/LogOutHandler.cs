using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.LogOut;

public class LogOutHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<LogOutRequest, Result>
{
    public async Task<Result> Handle(LogOutRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserJwtId = currentAccountService.GetTokenIdentifierOrThrow();

        var currentSession = await context.UserSessions.FindAsync([currentUserJwtId], cancellationToken);
        if (currentSession is null)
            throw new Exception();
        
        context.UserSessions.Remove(currentSession);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}