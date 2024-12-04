namespace Domain._Shared.Services.Auth;

public interface ICurrentAccountService
{
    public string? GetId();
    public List<string> GetRoles();
}