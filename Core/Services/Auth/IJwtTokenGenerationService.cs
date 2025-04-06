namespace Core.Services.Auth;

public interface IJwtTokenGenerationService
{
     public string Generate(AccountData accountData, Guid newTokenId);
}