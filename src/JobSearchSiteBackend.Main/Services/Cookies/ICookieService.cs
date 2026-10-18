namespace JobSearchSiteBackend.Main.Services.Cookies;

public interface ICookieService
{
    void SetAuthCookie(string token);
    void RemoveAuthCookie();
}