using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobCardDto(
    long Id,
    long CompanyId,
    string? CompanyAvatarLink,
    string CompanyName,
    ICollection<LocationDto> Locations,
    string Title,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    JobSalaryInfoDto? SalaryInfoDto,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    bool IsBookmarked);