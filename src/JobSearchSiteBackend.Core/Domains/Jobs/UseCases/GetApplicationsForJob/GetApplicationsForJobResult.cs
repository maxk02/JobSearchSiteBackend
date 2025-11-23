using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public record GetApplicationsForJobResult(ICollection<JobApplicationForManagersDto> JobApplications);