using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;

public record AddJobCommand(
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
    ICollection<long> LocationIds) : IRequest<Result<AddJobResult>>;