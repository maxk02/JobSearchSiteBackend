using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;

public record GetCompanyJobManagementCardDtosQuery(
    long CompanyId,
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    long? LocationId,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds) : IRequest<Result<GetCompanyJobManagementCardDtosResult>>;