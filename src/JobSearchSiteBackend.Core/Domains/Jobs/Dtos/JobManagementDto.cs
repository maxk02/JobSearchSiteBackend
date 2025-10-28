using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobManagementDto(
    long Id,
    long CompanyId,
    string? CompanyLogoLink,
    string CompanyName,
    string? CompanyDescription,
    ICollection<LocationDto> Locations, // todo stringlocationdto??
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
    ICollection<long>? ContractTypeIds,
    //
    long FolderId,
    string FolderName,
    ICollection<long> ClaimIds,
    bool IsPublic,
    int TimeRangeOptionId);