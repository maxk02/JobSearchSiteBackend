namespace Core.Services.Cookies;

public interface ICookieService
{
    void SetAuthCookie(string token);
    void RemoveAuthCookie(string token);
}