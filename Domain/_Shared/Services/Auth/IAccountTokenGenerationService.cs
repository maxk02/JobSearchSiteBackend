namespace Domain._Shared.Services.Auth;

public interface IAccountTokenGenerationService
{
     // public string? Generate(long userId, ICollection<string> roles);
     public string Generate(AccountData accountData);
}