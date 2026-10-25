using JobSearchSiteBackend.Main.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Locations.UseCases.GetLocations;

public record GetLocationsResult(ICollection<LocationDto> Locations);