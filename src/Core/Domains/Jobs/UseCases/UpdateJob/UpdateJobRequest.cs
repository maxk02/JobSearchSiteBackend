using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.Jobs.UseCases.UpdateJob;

public record UpdateJobRequest(long Id,
    long? JobFolderId,
    long? CategoryId,
    string? Title,
    string? Description,
    bool? IsPublic,
    DateTime? NewDateTimeExpiringUtc,
    ICollection<string>? Responsibilities,
    ICollection<string>? Requirements,
    ICollection<string>? Advantages,
    JobSalaryInfoDto? SalaryInfo,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? ContractTypeIds,
    ICollection<long>? LocationIds) : IRequest<Result>;