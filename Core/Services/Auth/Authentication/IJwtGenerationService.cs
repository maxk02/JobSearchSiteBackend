namespace Core.Services.Auth.Authentication;

public interface IJwtGenerationService
{
     // public string? Generate(long userId, ICollection<string> roles);
     public string Generate(AccountData accountData);
}