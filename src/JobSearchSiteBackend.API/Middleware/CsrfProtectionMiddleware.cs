using Microsoft.AspNetCore.Antiforgery;

namespace JobSearchSiteBackend.API.Middleware;

public class CsrfProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAntiforgery _antiforgery;
    private readonly string[] _safeMethods = ["GET", "HEAD", "OPTIONS", "TRACE"];

    public CsrfProtectionMiddleware(RequestDelegate next, IAntiforgery antiforgery)
    {
        _next = next;
        _antiforgery = antiforgery;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_safeMethods.Contains(context.Request.Method))
        {
            await _next(context);
            return;
        }
        
        try
        {
            await _antiforgery.ValidateRequestAsync(context);
            await _next(context);
        }
        catch (AntiforgeryValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("CSRF token validation failed.");
            return;
        }
    }
}