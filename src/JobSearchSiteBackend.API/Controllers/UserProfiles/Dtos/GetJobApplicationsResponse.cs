using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetJobApplicationsResponse(
    ICollection<JobApplicationInUserProfileDto> JobApplications,
    PaginationResponse PaginationResponse);