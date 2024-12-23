namespace Core.Domains.Locations.UseCases.GetLocationById;

public record GetLocationByIdResponse(long CountryId, string Name,
    ICollection<string> Subdivisions, string? Description, string? Code);