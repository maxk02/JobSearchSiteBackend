using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;

public record GetCompanyJobManagementCardDtosQuery(
    long CompanyId,
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    long? LocationId,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetCompanyJobManagementCardDtosResult>>;