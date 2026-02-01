using JobSearchSiteBackend.API.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyJobManagementCardDtosRequest(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    long? LocationId,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? EmploymentOptionIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? CategoryIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? ContractTypeIds);