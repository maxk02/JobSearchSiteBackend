using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;

public record GetCompanyBalanceQuery(long Id) : IRequest<Result<GetCompanyBalanceResult>>;