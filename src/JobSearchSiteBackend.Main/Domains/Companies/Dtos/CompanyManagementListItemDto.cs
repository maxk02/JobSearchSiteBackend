namespace JobSearchSiteBackend.Main.Domains.Companies.Dtos;

public record CompanyManagementListItemDto(long Id, string Name, long CountryId, string? AvatarLink);