using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobDetailedDto(
    long Id,
    long CompanyId,
    string? CompanyAvatarLink,
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
    bool IsBookmarked,
    long? ApplicationId);