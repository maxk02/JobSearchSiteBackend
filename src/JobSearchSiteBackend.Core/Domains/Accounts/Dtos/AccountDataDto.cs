using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

public record AccountDataDto(
    long Id,
    string Email,
    string? FullName,
    string? AvatarLink,
    ICollection<CompanyDto> CompaniesManaged);