using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

public record JobManagementCardDto(
    long Id,
    ICollection<LocationDto> Locations,
    string Title,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    bool IsBookmarked,
    bool IsPublic
); // todo check on frontend, no time range option id