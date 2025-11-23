using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetBookmarkedJobsResponse(
    ICollection<JobCardDto> JobInfos,
    PaginationResponse PaginationResponse);