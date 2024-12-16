namespace Core.Services.Auth.Authentication;

public interface ICurrentAccountService
{
    public long? GetId();
    public long GetIdOrThrow();
    public string? GetEmail();
    public string GetEmailOrThrow();
    public List<string>? GetRoles();
    public List<string> GetRolesOrThrow();
    public bool IsLoggedIn();
    public void ThrowIfNotLoggedIn();
}