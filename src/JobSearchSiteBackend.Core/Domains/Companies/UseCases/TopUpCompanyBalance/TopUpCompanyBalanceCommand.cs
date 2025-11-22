using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.TopUpCompanyBalance;

public record TopUpCompanyBalanceCommand(long Id, decimal Amount, string CurrencyCode) : IRequest<Result>;