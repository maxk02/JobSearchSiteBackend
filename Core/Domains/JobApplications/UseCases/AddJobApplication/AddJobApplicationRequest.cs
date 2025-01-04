using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddJobApplication;

public record AddJobApplicationRequest(long UserId, long JobId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddJobApplicationResponse>>;