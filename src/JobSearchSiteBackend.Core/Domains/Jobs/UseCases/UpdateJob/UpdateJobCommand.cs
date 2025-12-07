using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;

public record UpdateJobCommand(
    long Id,
    long? JobFolderId,
    long? CategoryId,
    string? Title,
    string? Description,
    bool? IsPublic,
    DateTime? NewDateTimeExpiringUtc,
    ICollection<string>? Responsibilities,
    ICollection<string>? Requirements,
    ICollection<string>? NiceToHaves,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    ICollection<long>? LocationIds) : IRequest<Result>;