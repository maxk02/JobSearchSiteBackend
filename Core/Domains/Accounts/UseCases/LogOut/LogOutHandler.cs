using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Ardalis.Result;
using Core.Services.Caching;
using Core.Services.Cookies;

namespace Core.Domains.Accounts.UseCases.LogOut;

public class LogOutHandler(ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICache<string, UserSession> sessionCache,
    ICookieService cookieService) : IRequestHandler<LogOutRequest, Result>
{
    public async Task<Result> Handle(LogOutRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserJwtId = currentAccountService.GetTokenIdentifierOrThrow();

        var currentSession = await context.UserSessions.FindAsync([currentUserJwtId], cancellationToken);
        if (currentSession is null)
            throw new Exception();
        
        await sessionCache.RemoveAsync($"user_session_{currentUserJwtId}");
        
        cookieService.RemoveAuthCookie(currentUserJwtId);
        
        context.UserSessions.Remove(currentSession);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}