using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobCardDto(
    long Id,
    string? CompanyLogoBase64,
    string CompanyName,
    ICollection<LocationDto> Locations,
    string Title,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? ContractTypeIds);