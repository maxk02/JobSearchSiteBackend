using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.Cookies;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogOut;

public class LogOutHandler(ICurrentAccountService currentAccountService,
    IUserSessionCacheRepository sessionCache,
    ICookieService cookieService) : IRequestHandler<LogOutCommand, Result>
{
    public async Task<Result> Handle(LogOutCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        var currentUserTokenId = currentAccountService.GetTokenIdentifierOrThrow();

        var currentSession =
            await sessionCache.GetSessionExpirationUtcAsync(currentUserId.ToString(), currentUserTokenId);
        
        if (currentSession is null)
            throw new Exception();
        
        await sessionCache.DeleteSessionAsync(currentUserId.ToString(), currentUserTokenId);
        
        cookieService.RemoveAuthCookie();
        
        return Result.Success();
    }
}