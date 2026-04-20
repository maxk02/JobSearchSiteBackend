using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetJobApplicationsRequest(int? StatusId, int Page, int Size);