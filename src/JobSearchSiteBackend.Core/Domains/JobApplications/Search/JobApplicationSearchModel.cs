using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Search;

public record JobApplicationSearchModel(
    long Id,
    long JobId,
    ICollection<string> FileTexts,
    DateTime DateTimeUpdatedUtc,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;