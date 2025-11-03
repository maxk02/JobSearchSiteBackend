using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;

public record GetCompanyJobsRequest(
    long CompanyId,
    string Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetCompanyJobsResponse>>;