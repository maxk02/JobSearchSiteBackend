using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public record GetJobsRequest(
    string Query,
    PaginationSpec PaginationSpec,
    long CountryId,
    bool? MustHaveSalaryRecord,
    EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetJobsResponse>>;