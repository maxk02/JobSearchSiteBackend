namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyBalanceTransactionDto(long Id, DateTime DateTimeCommittedUtc, long? UserId,
    string? UserFullName, string? UserEmail, decimal Amount, long CurrencyId, string Description);