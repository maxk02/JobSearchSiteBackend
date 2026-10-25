using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJobs;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobs;

public record GetCompanyJobsQuery(
    long CompanyId,
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetCompanyJobsResult>>;