using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public record GetApplicationsForJobResult(ICollection<JobApplicationForManagersDto> JobApplications,
    PaginationResponse PaginationResponse);