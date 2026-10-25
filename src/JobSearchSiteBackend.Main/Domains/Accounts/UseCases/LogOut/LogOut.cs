
using JobSearchSiteBackend.Main.Attributes;
using JobSearchSiteBackend.Main.Domains.Accounts.Caching;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Services.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.LogOut;

public static class LogOut
{
    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("logout", Handle)
            .WithMetadata(new AllowUnconfirmedEmailAttribute());

    public static async Task<Ok> Handle(
        ICurrentAccountService currentAccountService,
        IUserSessionCacheRepository sessionCache,
        ICookieService cookieService,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        var currentUserTokenId = currentAccountService.GetTokenIdentifierOrThrow();

        var currentSession = await sessionCache.GetSessionExpirationUtcAsync(
            currentUserId.ToString(),
            currentUserTokenId);

        if (currentSession is null)
            throw new InvalidOperationException("Active session not found.");

        await sessionCache.DeleteSessionAsync(currentUserId.ToString(), currentUserTokenId);
        cookieService.RemoveAuthCookie();

        return TypedResults.Ok();
    }
}