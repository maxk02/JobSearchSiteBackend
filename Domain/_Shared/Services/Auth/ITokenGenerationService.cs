namespace Domain._Shared.Services.Auth;

public interface ITokenGenerationService
{
     // public string? Generate(long userId, ICollection<string> roles);
     public string Generate(UserClaimsDto userClaimsDto);
}