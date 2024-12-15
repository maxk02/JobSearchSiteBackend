namespace Core.Services.Auth;

public record AccountData(long Id, string Email, ICollection<string> Roles);