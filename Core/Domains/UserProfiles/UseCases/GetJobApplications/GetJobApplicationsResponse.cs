using Core.Domains._Shared.Pagination;
using Core.Domains.JobApplications.Dtos;
using Core.Domains.PersonalFiles.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsResponse(
    ICollection<JobApplicationDto> JobApplicationDtos,
    PaginationResponse PaginationResponse);