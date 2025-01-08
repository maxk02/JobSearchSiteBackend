using Core.Persistence.EfCore;
using Core.Services.Auth;

namespace API.Middleware.Auth;

public class CheckUserTokenMiddleware(RequestDelegate next, ICurrentAccountService currentAccountService)
{
    public async Task InvokeAsync(HttpContext httpContext, MainDataContext dbContext)
    {
        var endpoint = httpContext.GetEndpoint();

        if (endpoint is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return;
        }
        
        var hasAuthorizeAttribute = endpoint.Metadata
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .Any();

        if (!hasAuthorizeAttribute)
        {
            await next(httpContext);
            return;
        }
        
        var cancellationToken = httpContext.RequestAborted;

        var tokenId = currentAccountService.GetTokenIdentifierOrThrow();
        var userId = currentAccountService.GetIdOrThrow();

        var session = await dbContext.UserSessions.FindAsync([tokenId], cancellationToken);

        if (session is null || session.UserId != userId || DateTime.UtcNow > session.ExpiresUtc)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        await next(httpContext);
    }
}