namespace Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;

public record UpdateApplicationFilesRequest(long ApplicationId, ICollection<long> PersonalFileIds);