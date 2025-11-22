namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;

public record GetCompanyBalanceResult(string CurrencyCode, decimal Balance);