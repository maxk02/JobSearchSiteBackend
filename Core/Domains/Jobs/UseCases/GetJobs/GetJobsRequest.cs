using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Ardalis.Result;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsRequest(
    string Query,
    PaginationSpec PaginationSpec,
    bool? MustHaveSalaryRecord,
    EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<long>? CountryIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetJobsResponse>>;