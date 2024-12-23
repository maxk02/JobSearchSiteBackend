namespace Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsResponse(long CountryId, string Name,
    ICollection<string> Subdivisions, string? Description, string? Code);