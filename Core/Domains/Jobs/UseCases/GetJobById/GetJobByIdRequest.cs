using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.GetJobById;

public record GetJobByIdRequest(long Id) : IRequest<Result<GetJobByIdResponse>>;