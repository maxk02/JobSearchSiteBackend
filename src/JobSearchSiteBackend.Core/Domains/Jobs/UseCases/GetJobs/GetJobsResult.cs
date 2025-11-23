using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsResult(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);