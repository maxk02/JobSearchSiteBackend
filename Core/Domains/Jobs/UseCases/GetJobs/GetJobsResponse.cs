using Core.Domains._Shared.Pagination;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsResponse(
    ICollection<JobInfoDto> JobInfoCards,
    PaginationResponse PaginationResponse);