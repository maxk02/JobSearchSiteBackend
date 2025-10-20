using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public record PersonalFileSearchModel(
    long Id,
    string TextContent,
    bool IsDeleted
) : ISearchModelWithId, IDeletableSearchModel;