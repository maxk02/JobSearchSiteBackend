using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public record UpdateJobApplicationFilesCommand(long Id, long LocationId,
    ICollection<long> PersonalFileIds) : IRequest<Result>;