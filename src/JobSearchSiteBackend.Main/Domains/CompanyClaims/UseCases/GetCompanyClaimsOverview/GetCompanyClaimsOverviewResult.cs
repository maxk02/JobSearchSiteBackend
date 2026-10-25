using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.CompanyClaims.Dtos;

namespace JobSearchSiteBackend.Main.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;

public record GetCompanyClaimsOverviewResult(
    ICollection<CompanyClaimOverviewDto> CompanyClaimOverviewDtos,
    PaginationResponse PaginationResponse);