using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddApplication;

public record AddApplicationRequest(long UserId, long JobId,
    ICollection<long> PersonalFileIds) : IRequest<Result<AddApplicationResponse>>;