using Core.Domains.Locations.Dtos;

namespace Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsResponse(ICollection<LocationDto> Locations);