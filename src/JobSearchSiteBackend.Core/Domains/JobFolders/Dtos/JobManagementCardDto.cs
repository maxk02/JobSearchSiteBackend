using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

public record JobManagementCardDto(
    long Id,
    string? CompanyLogoLink,
    ICollection<LocationDto> Locations, //todo check
    string Title,
    string DateTimePublishedUtc,
    string DateTimeExpiringUtc,
    JobSalaryInfo? SalaryInfo,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    bool IsPublic,
    int TimeRangeOptionId
);