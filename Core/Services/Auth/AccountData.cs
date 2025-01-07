namespace Core.Services.Auth;

public record AccountData(long Id, ICollection<string> Roles);