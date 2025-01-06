using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsRequest(long CountryId, string Query) : IRequest<GetLocationsResponse>;