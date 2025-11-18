using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;

public record GetCompanyBalanceRequest(long Id) : IRequest<Result<GetCompanyBalanceResponse>>;