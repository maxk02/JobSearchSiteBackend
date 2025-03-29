using Core.Domains._Shared.Pagination;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public record GetBookmarkedJobsResponse(
    ICollection<JobInfoDto> JobInfoDtos,
    PaginationResponse PaginationResponse);