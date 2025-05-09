using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsResponse(
    ICollection<JobApplicationInUserProfileDto> JobApplications,
    PaginationResponse PaginationResponse);