using Core.Domains.Accounts;
using Core.Persistence;
using Core.Services.Auth;
using Core.Services.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace JobSearchSiteBackend.API.Middleware;

public class CheckUserTokenMiddleware(
    RequestDelegate next,
    ICurrentAccountService currentAccountService,
    ICache<string, UserSession> sessionCache)
{
    public async Task InvokeAsync(HttpContext httpContext, MainDataContext dbContext)
    {
        var endpoint = httpContext.GetEndpoint();

        if (endpoint is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return;
        }
        
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;

        if (!isAuthenticated)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        var cancellationToken = httpContext.RequestAborted;

        var tokenId = currentAccountService.GetTokenIdentifierOrThrow();
        var userId = currentAccountService.GetIdOrThrow();

        var session = await sessionCache.GetAsync($"user_session_{tokenId}"); 
        
        if (session is null)
        {
            session = await dbContext.UserSessions.FindAsync([tokenId], cancellationToken);

            if (session is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            
            await sessionCache.SetAsync($"user_session_{tokenId}", session, new CacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(session.ExpiresUtc, TimeSpan.Zero),
            });
        }
        
        if (session.UserId != userId || DateTime.UtcNow > session.ExpiresUtc)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        await next(httpContext);
    }
}