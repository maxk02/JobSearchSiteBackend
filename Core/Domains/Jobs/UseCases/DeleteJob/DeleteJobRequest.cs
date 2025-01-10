using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.DeleteJob;

public record DeleteJobRequest(long JobId) : IRequest<Result>;