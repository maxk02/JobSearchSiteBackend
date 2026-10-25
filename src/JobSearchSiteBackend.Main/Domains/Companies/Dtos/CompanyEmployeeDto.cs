namespace JobSearchSiteBackend.Main.Domains.Companies.Dtos;

public record CompanyEmployeeDto(long Id, string Email, string FullName, string? AvatarLink);