namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyManagementDetailedDto(
    long Id,
    string Name,
    string? Description,
    long CountryId,
    string? AvatarLink,
    ICollection<long> ClaimIds,
    string CountrySpecificFieldsJson);