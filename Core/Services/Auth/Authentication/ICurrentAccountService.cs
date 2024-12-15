namespace Core.Services.Auth.Authentication;

public interface ICurrentAccountService
{
    public long? GetId();
    public string? GetEmail();
    public List<string>? GetRoles();
}