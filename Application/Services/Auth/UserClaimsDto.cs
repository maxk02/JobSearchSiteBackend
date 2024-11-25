namespace Application.Services.Auth;

public record UserClaimsDto(string Id, ICollection<string> Roles);