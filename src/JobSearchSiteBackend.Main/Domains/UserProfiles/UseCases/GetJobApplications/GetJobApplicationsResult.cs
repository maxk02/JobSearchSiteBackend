using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsResult(
    ICollection<JobApplicationInUserProfileDto> JobApplications,
    PaginationResponse PaginationResponse);