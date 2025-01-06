namespace Core.Services.Auth;

public interface IJwtGenerationService
{
     // public string? Generate(long userId, ICollection<string> roles);
     public string Generate(AccountData accountData);
}