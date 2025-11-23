using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetApplicationsForJobResponse(ICollection<JobApplicationForManagersDto> JobApplications);