namespace JobSearchSiteBackend.API.Controllers.Locations.Dtos;

public record GetLocationsRequest(long CountryId, string Query);