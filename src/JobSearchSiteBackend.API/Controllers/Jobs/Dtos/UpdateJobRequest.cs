using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record UpdateJobRequest(
    long? JobFolderId,
    long? CategoryId,
    string? Title,
    string? Description,
    int TimeRangeOptionId,
    bool? IsPublic,
    DateTime? NewDateTimeExpiringUtc,
    ICollection<string>? Responsibilities,
    ICollection<string>? Requirements,
    ICollection<string>? Advantages,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? ContractTypeIds,
    ICollection<long>? LocationIds);