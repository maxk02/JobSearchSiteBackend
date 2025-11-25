namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record UpdateJobApplicationFilesRequest(ICollection<long> PersonalFileIds);