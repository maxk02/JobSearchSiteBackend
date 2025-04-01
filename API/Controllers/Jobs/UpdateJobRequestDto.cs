using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Jobs.Dtos;

namespace API.Controllers.Jobs;

public record UpdateJobRequestDto(
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