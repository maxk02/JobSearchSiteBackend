using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsResult(ICollection<LocationDto> Locations);