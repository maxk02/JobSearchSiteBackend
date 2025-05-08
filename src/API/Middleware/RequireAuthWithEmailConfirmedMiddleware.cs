using API.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace API.Middleware;

public class RequireAuthWithEmailConfirmedMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var endpoint = httpContext.GetEndpoint();

        if (endpoint is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return;
        }
        
        // Allow anon
        var allowAnonymous = endpoint.Metadata
            .GetMetadata<IAllowAnonymous>() != null;

        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
            
        if (!allowAnonymous && !isAuthenticated)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // Check for [AllowUnconfirmedEmail] attribute
        var hasAllowUnconfirmedEmailAttribute = endpoint.Metadata
            .OfType<AllowUnconfirmedEmailAttribute>()
            .Any();

        if (!hasAllowUnconfirmedEmailAttribute)
        {
            var emailConfirmedClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "EmailConfirmed")?.Value;

            if (string.IsNullOrEmpty(emailConfirmedClaim)
                || !bool.TryParse(emailConfirmedClaim, out var isEmailConfirmed)
                || !isEmailConfirmed)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }

        await next(httpContext);
    }
}