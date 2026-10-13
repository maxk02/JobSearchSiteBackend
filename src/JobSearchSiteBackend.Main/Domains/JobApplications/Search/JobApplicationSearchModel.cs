using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Search;

public record JobApplicationSearchModel(
    long Id,
    long JobId,
    ICollection<string> FileTexts,
    Guid VersionId,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;