namespace Core.Domains.Companies.Dtos;

public record CompanyInfoDto(long Id, string Name, long CountryId, string? LogoLink);