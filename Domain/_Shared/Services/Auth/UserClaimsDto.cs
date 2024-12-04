namespace Domain._Shared.Services.Auth;

public record UserClaimsDto(string Id, ICollection<string> Roles);