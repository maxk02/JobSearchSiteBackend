namespace Core.Domains.UserProfiles.Dtos;

public record CompanyShortDto(long Id, string Name, string? Description, long CountryId);