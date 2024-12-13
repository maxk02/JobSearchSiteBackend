namespace API.Services.Auth.CurrentAccount;

public interface ICurrentAccountService
{
    public long? GetId();
    public string? GetEmail();
    public List<string>? GetRoles();
}