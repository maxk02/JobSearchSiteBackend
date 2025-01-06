namespace Core.Domains.JobApplications.Dtos;

public record JobApplicationDto(long Id, long UserId, long JobId, string Status);