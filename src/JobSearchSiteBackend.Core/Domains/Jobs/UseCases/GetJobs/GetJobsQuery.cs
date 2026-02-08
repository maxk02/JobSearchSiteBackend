using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsQuery(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? LocationIds,
    ICollection<long>? CountryIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetJobsResult>>;