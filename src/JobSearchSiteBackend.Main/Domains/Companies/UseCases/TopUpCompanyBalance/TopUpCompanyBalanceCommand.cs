using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.TopUpCompanyBalance;

public record TopUpCompanyBalanceCommand(long Id, decimal Amount, long CurrencyId) : IRequest<Result>;