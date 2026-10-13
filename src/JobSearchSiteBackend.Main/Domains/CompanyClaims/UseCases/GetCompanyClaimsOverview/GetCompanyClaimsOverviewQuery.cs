using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;

public record GetCompanyClaimsOverviewQuery(
    long CompanyId,
    ICollection<long>? CompanyClaimIds,
    string? UserQuery,
    int Page,
    int Size) : IRequest<Result<GetCompanyClaimsOverviewResult>>;