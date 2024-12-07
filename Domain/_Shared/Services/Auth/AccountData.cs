namespace Domain._Shared.Services.Auth;

public record AccountData(string Id, string Email, ICollection<string> Roles);