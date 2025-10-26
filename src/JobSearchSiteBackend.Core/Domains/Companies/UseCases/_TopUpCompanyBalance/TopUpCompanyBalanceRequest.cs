using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases._TopUpCompanyBalance;

public record TopUpCompanyBalanceRequest(long Id, decimal BalanceUpdate) : IRequest<Result>;