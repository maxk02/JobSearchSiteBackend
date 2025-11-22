using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record TopUpCompanyBalanceRequest(decimal Amount, string CurrencyCode);