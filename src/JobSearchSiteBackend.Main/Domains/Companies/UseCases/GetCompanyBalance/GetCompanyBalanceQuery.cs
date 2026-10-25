using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyBalance;

public record GetCompanyBalanceQuery(long Id) : IRequest<Result<GetCompanyBalanceResult>>;