using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Locations.Dtos;

public record GetLocationsResponse(ICollection<LocationDto> Locations);