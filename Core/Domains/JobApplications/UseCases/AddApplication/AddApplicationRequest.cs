namespace Core.Domains.JobApplications.UseCases.AddApplication;

public record AddApplicationRequest(long UserId, long JobId, ICollection<long> PersonalFileIds);