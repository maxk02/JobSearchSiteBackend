﻿using Core.Services.Cookies;

namespace JobSearchSiteBackend.API.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    public void SetAuthCookie(string token)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("Cannot set cookie auth unless HTTP context is set.");
        }
        
        httpContextAccessor.HttpContext.Response.Cookies.Append(
            "auth_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            }
        );
    }
    
    public void RemoveAuthCookie(string token)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("Cannot set cookie auth unless HTTP context is set.");
        }
        
        httpContextAccessor.HttpContext.Response.Cookies.Delete("auth_token");
    }
}