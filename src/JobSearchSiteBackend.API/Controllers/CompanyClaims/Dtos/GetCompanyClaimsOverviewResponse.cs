using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;

public record GetCompanyClaimsOverviewResponse(
    ICollection<CompanyClaimOverviewDto> CompanyClaimOverviewDtos,
    PaginationResponse PaginationResponse);