using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.Dtos;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;

public record GetJobFolderClaimsOverviewResponse(
    ICollection<JobFolderClaimOverviewDto> JobFolderClaimOverviewDtos,
    PaginationResponse PaginationResponse);