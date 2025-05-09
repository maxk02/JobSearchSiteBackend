using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompanyBalance;

public record TopUpCompanyBalanceRequest(long Id, decimal BalanceUpdate) : IRequest<Result>;