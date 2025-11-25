using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;

public record GetCompanyClaimsOverviewRequest(
    ICollection<long>? CompanyClaimIds,
    string? UserQuery,
    int Page,
    int Size) : IRequest<Result<GetCompanyClaimsOverviewResponse>>;