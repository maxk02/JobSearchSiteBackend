namespace API.Services.Auth;

public record AccountData(long Id, string Email, ICollection<string> Roles);