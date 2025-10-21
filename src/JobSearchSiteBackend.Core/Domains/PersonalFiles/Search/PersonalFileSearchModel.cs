using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public record PersonalFileSearchModel(
    long Id,
    string TextContent,
    DateTime DateTimeUpdatedUtc,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;