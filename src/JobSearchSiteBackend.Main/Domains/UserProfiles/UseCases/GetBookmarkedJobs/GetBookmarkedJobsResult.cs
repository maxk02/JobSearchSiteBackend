using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public record GetBookmarkedJobsResult(
    ICollection<JobCardDto> JobInfos,
    PaginationResponse PaginationResponse);