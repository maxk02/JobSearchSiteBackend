using JobSearchSiteBackend.API.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetApplicationsForJobRequest(
    long LocationId,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? StatusIds,
    string? Query,
    string? SortOption,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<string>? IncludedTags,
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<string>? ExcludedTags,
    int Page,
    int Size);