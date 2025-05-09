namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyManagementListItemDto(long Id, string Name, long CountryId, string? LogoLink);