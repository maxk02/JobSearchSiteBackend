using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;

public record AddJobApplicationRequest(long UserId, long JobId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddJobApplicationResponse>>;