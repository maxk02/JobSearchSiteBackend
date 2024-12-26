using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateApplicationStatus;

public record UpdateApplicationStatusRequest(long ApplicationId, string Status) : IRequest<Result>;