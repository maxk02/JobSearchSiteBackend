using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;

public record GetCompanyClaimsOverviewResult(
    ICollection<CompanyClaimOverviewDto> CompanyClaimOverviewDtos,
    PaginationResponse PaginationResponse);