using Core.Domains.Locations.Dtos;

namespace Core.Domains.Jobs.Dtos;

public record JobCardDto(
    long Id,
    string? CompanyLogoLink,
    string CompanyName,
    ICollection<LocationDto> Locations,
    string Title,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentTypeIds);