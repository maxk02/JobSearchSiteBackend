using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetApplicationsForJob;

public record GetApplicationsForJobResult(ICollection<JobApplicationForManagersDto> JobApplications,
    PaginationResponse PaginationResponse);