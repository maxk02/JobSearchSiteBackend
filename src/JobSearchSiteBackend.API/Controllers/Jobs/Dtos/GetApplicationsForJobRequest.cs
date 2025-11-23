namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetApplicationsForJobRequest(
    ICollection<long> StatusIds,
    string? Query,
    int SortOption,
    ICollection<string> IncludedTags,
    ICollection<string> ExcludedTags,
    int Page,
    int Size);