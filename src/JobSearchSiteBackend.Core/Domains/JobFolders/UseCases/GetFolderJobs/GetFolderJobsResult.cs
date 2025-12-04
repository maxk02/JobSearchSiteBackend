using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetFolderJobs;

public record GetFolderJobsResult(
    ICollection<JobManagementCardDto> Jobs,
    PaginationResponse PaginationResponse);