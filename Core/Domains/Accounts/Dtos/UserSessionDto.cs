namespace Core.Domains.Accounts.Dtos;

public record UserSessionDto(string TokenId, DateTime FirstTimeIssuedUtc);