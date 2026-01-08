using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobManagementDto(
    long Id,
    long CompanyId,
    string? CompanyLogoLink,
    string CompanyName,
    string? CompanyDescription,
    ICollection<LocationDto> Locations,
    long CategoryId,
    string Title,
    string? Description,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    JobSalaryInfoDto? SalaryInfoDto,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    bool IsPublic,
    ICollection<long> ClaimIds);