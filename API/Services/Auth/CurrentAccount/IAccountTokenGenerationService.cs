namespace API.Services.Auth.CurrentAccount;

public interface IAccountTokenGenerationService
{
     // public string? Generate(long userId, ICollection<string> roles);
     public string Generate(AccountData accountData);
}