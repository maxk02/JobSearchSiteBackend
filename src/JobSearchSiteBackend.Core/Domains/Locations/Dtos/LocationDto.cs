namespace JobSearchSiteBackend.Core.Domains.Locations.Dtos;

public record LocationDto(long Id, long CountryId, string FullName, string? Description, string? Code);