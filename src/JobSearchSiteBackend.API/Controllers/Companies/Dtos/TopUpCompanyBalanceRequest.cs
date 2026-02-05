namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record TopUpCompanyBalanceRequest(decimal Amount, long CurrencyId);