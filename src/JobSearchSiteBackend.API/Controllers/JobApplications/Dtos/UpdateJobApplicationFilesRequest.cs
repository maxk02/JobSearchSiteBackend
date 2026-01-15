namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record UpdateJobApplicationFilesRequest(long LocationId, ICollection<long> PersonalFileIds);