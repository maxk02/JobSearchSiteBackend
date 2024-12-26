using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Locations.UseCases.GetLocationById;

public record GetLocationByIdRequest(long Id) : IRequest<Result<GetLocationByIdResponse>>;