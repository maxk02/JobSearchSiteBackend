namespace Application.Services.Auth;

public interface ITokenGenerationService
{
     public string? Generate(long userId, ICollection<string> roles);
}