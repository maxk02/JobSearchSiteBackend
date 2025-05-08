namespace Core.Domains.Locations.Dtos;

public record LocationDto(long Id, long CountryId, string Name,
    ICollection<string> Subdivisions, string? Description, string? Code);