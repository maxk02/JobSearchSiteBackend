using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.AddJobApplication;

public record AddJobApplicationRequest(long UserId, long JobId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddJobApplicationResponse>>;