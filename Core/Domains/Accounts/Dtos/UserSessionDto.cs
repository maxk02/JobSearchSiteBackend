namespace Core.Domains.Accounts.Dtos;

public record UserSessionDto(string TokenId, DateTime FirstTimeIssuedUtc,
    string? LastDevice, string? LastOs, string? LastClient);