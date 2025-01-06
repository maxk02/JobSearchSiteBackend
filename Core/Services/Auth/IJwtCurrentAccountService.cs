namespace Core.Services.Auth;

public interface IJwtCurrentAccountService
{
    public string GetTokenIdentifierOrThrow(); 
    public long? GetId();
    public long GetIdOrThrow();
    public string? GetEmail();
    public string GetEmailOrThrow();
    public List<string>? GetRoles();
    public List<string> GetRolesOrThrow();
    public bool IsLoggedIn();
    public void ThrowIfNotLoggedIn();
}