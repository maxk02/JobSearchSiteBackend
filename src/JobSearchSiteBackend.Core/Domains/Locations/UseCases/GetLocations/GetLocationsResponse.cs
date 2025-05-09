using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsResponse(ICollection<LocationDto> Locations);