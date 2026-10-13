using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;

public record AddJobApplicationCommand(long JobId, long LocationId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddJobApplicationResult>>;