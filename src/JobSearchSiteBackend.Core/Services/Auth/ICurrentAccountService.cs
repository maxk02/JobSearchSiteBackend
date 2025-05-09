namespace JobSearchSiteBackend.Core.Services.Auth;

public interface ICurrentAccountService
{
    public string GetTokenIdentifierOrThrow(); 
    public long? GetId();
    public long GetIdOrThrow();
    public List<string>? GetRoles();
    public List<string> GetRolesOrThrow();
    public bool IsLoggedIn();
    public void ThrowIfNotLoggedIn();
}