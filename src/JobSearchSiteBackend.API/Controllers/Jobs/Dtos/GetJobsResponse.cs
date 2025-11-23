using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetJobsResponse(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);