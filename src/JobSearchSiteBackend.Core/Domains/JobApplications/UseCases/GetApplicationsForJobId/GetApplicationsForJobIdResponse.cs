using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

public record GetApplicationsForJobIdResponse(
    ICollection<JobApplicationForManagersDto> JobApplications,
    PaginationResponse PaginationResponse);