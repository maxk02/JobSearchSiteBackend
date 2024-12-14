namespace Core._Shared.Services.Auth;

public interface ICurrentAccountService
{
    public long? GetId();
    public string? GetEmail();
    public List<string>? GetRoles();
}