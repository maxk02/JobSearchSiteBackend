using Core.Domains._Shared.Pagination;
using Core.Domains.JobApplications.Dtos;

namespace Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

public record GetApplicationsForJobIdResponse(
    ICollection<JobApplicationForManagersDto> JobApplications,
    PaginationResponse PaginationResponse);