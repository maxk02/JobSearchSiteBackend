namespace JobSearchSiteBackend.Main.Services.Auth;

public record AccountData(long Id, bool EmailConfirmed, ICollection<string> Roles);