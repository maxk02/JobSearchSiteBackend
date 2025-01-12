using Core.Domains._Shared.Pagination;
using Core.Domains.JobApplications.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;

public record GetJobApplicationsForUserResponse(
    ICollection<JobApplicationInUserProfileDto> JobApplicationDtos,
    PaginationResponse PaginationResponse);