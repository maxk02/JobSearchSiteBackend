using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetApplicationsForJob;

public record GetApplicationsForJobQuery(
    long Id,
    long LocationId,
    ICollection<long> StatusIds,
    string? Query,
    string? SortOption,
    ICollection<string> IncludedTags,
    ICollection<string> ExcludedTags,
    int Page,
    int Size) : IRequest<Result<GetApplicationsForJobResult>>;