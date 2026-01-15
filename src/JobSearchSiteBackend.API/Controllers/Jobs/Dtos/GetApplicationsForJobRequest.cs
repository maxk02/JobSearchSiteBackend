namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetApplicationsForJobRequest(
    ICollection<long>? StatusIds,
    string? Query,
    string? SortOption,  // todo frontend
    ICollection<string>? IncludedTags,
    ICollection<string>? ExcludedTags,
    int Page,
    int Size);