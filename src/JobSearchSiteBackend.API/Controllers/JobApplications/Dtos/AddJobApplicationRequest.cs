namespace JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;

public record AddJobApplicationRequest(long JobId,
    ICollection<long> PersonalFileIds);