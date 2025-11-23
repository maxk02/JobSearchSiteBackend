using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record AddJobRequest(
    long JobFolderId,
    long CategoryId,
    string Title,
    string Description,
    bool IsPublic,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    JobSalaryInfoDto? JobSalaryInfoDto,
    ICollection<long> EmploymentTypeIds,
    ICollection<long> ContractTypeIds,
    ICollection<long> LocationIds);