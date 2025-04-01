using Core.Domains._Shared.Pagination;
using Core.Domains.JobApplications.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsResponse(
    ICollection<JobApplicationInUserProfileDto> JobApplications,
    PaginationResponse PaginationResponse);