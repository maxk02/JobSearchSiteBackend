using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.AddJob;

public record AddJobCommand(
    long CompanyId,
    long CategoryId,
    string Title,
    string? Description,
    bool IsPublic,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    JobSalaryInfoDto? JobSalaryInfoDto,
    ICollection<long> EmploymentOptionIds,
    ICollection<long> ContractTypeIds,
    ICollection<long> LocationIds) : IRequest<Result<AddJobResult>>;