namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;

public record GetCompanyBalanceResponse(string CurrencyCode, decimal Balance);