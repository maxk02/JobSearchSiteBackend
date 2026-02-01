using JobSearchSiteBackend.API.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetJobsRequest(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? EmploymentOptionIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? LocationIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? CategoryIds,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? ContractTypeIds);