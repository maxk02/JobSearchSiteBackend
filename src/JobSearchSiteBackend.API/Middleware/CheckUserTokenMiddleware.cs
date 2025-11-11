using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace JobSearchSiteBackend.API.Middleware;

public class CheckUserTokenMiddleware(
    RequestDelegate next,
    ICurrentAccountService currentAccountService,
    IUserSessionCacheRepository sessionCache)
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

        var expirationUtc = await sessionCache.GetSessionExpirationUtcAsync(userId.ToString(), tokenId); 
        
        if (expirationUtc is null || expirationUtc <= DateTime.UtcNow)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        await sessionCache.ProlongSessionAsync(userId.ToString(), tokenId, expirationUtc.Value.AddMonths(1));
        
        await next(httpContext);
    }
}