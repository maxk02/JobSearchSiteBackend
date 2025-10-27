using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases._GetApplicationsForJobId;

public record GetApplicationsForJobIdResponse(
    ICollection<JobApplicationForManagersDto> JobApplications,
    PaginationResponse PaginationResponse);