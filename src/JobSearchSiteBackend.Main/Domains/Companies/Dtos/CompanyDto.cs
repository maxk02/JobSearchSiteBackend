namespace JobSearchSiteBackend.Main.Domains.Companies.Dtos;

public record CompanyDto(long Id, string Name, string? Description, long CountryId, string? AvatarLink);