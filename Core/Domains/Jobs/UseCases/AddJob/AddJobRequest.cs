using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Ardalis.Result;

namespace Core.Domains.Jobs.UseCases.AddJob;

public record AddJobRequest(
    long JobFolderId,
    long CategoryId,
    string Title,
    string Description,
    bool IsPublic,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> Advantages,
    SalaryRecord SalaryRecord,
    EmploymentTypeRecord EmploymentTypeRecord,
    ICollection<long> ContractTypeIds,
    ICollection<long> LocationIds) : IRequest<Result>;