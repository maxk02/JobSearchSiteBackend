using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public record GetApplicationsForJobRequest(
    long Id,
    ICollection<long> StatusIds,
    string? Query,
    int SortOption, // todo
    ICollection<string> IncludedTags,
    ICollection<string> ExcludedTags,
    int Page,
    int Size) : IRequest<Result<GetApplicationsForJobResponse>>;