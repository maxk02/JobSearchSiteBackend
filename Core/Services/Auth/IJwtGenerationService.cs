namespace Core.Services.Auth;

public interface IJwtGenerationService
{
     public string Generate(AccountData accountData, Guid newTokenId);
}