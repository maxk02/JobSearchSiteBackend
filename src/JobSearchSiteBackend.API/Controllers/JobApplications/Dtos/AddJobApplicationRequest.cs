namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record AddJobApplicationRequest(long JobId, long LocationId,
    ICollection<long> PersonalFileIds);