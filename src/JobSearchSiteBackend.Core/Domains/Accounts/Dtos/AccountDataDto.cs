using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

public record AccountDataDto(
    long Id,
    string Email,
    string? FullName,
    string? AvatarBase64,
    ICollection<CompanyDto> CompaniesManaged);