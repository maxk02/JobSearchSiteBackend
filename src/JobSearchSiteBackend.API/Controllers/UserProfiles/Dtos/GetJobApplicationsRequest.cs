using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetJobApplicationsRequest(JobApplicationStatus? StatusId, int Page, int Size);