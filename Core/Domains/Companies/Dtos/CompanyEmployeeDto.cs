namespace Core.Domains.Companies.Dtos;

public record CompanyEmployeeDto(long Id, string Name, long CountryId, string? LogoLink);