using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJobs;

public record GetJobsResult(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);