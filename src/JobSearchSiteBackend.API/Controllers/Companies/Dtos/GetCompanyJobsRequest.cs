using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyJobsRequest(
    string Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds);