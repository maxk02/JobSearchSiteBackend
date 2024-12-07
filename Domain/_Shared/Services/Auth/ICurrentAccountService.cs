namespace Domain._Shared.Services.Auth;

public interface ICurrentAccountService
{
    public string? GetId();
    public string? GetEmail();
    public List<string> GetRoles();
}