using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobDetailedDto(
    long Id,
    string? CompanyLogoLink,
    string CompanyName,
    ICollection<LocationDto> Locations,
    long CategoryId,
    string Title,
    string? Description,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? ContractTypeIds);