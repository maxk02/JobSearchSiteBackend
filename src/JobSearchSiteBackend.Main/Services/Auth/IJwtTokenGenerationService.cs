namespace JobSearchSiteBackend.Main.Services.Auth;

public interface IJwtTokenGenerationService
{
     public string Generate(AccountData accountData, Guid newTokenId);
}