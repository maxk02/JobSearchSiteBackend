namespace Core.Domains.JobApplications.UseCases.UpdateApplicationStatus;

public record UpdateApplicationStatusRequest(long ApplicationId, string Status);