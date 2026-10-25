using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;

public record GetCompanyClaimsOverviewQuery(
    long CompanyId,
    ICollection<long>? CompanyClaimIds,
    string? UserQuery,
    int Page,
    int Size) : IRequest<Result<GetCompanyClaimsOverviewResult>>;