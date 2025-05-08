using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Jobs.UseCases.GetJobById;

public record GetJobByIdRequest(long Id) : IRequest<Result<GetJobByIdResponse>>;