using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Locations.UseCases.GetLocationById;

public record GetLocationByIdRequest(long Id) : IRequest<Result<GetLocationByIdResponse>>;