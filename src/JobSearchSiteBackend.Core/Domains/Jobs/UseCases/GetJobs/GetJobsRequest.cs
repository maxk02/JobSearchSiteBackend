using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsRequest(
    string Query,
    PaginationSpec PaginationSpec,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CountryIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetJobsResponse>>;