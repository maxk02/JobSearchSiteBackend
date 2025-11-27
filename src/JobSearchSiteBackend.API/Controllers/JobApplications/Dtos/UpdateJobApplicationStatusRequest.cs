using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record UpdateJobApplicationStatusRequest(JobApplicationStatus StatusId);