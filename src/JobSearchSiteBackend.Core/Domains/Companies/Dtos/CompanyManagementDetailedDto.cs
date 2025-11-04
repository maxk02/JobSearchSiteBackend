namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyManagementDetailedDto(
    long Id,
    string Name,
    string? Description,
    long CountryId,
    string? LogoLink,
    string Nip,
    ICollection<long> ClaimIds);