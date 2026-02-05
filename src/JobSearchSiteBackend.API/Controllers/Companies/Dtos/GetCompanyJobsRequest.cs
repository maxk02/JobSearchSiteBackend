using JobSearchSiteBackend.API.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyJobsRequest(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? EmploymentTypeIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? CategoryIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? ContractTypeIds);