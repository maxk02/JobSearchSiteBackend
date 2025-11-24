using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;

public record GetJobFolderClaimsOverviewResult(
    ICollection<JobFolderClaimOverviewDto> JobFolderClaimOverviewDtos,
    PaginationResponse PaginationResponse);