using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Jobs.UseCases.DeleteJob;

public record DeleteJobRequest(long JobId) : IRequest<Result>;