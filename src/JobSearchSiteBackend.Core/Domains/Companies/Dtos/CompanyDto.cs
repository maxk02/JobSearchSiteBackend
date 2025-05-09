namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyDto(long Id, string Name, long CountryId, string? LogoLink);