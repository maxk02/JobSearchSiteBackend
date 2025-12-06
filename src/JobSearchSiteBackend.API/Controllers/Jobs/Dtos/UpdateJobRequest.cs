using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record UpdateJobRequest(
    string? Title,
    long? FolderId,
    long? CategoryId,
    string? Description,
    // todo without time range option id
    bool? IsPublic,
    DateTime? DateTimeExpiringUtc,
    ICollection<string>? Responsibilities,
    ICollection<string>? Requirements,
    ICollection<string>? NiceToHaves,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    ICollection<long>? LocationIds);