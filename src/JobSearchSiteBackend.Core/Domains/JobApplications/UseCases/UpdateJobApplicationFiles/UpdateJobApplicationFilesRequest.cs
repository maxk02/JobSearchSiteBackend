using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public record UpdateJobApplicationFilesRequest(long Id,
    ICollection<long> PersonalFileIds) : IRequest<Result>;