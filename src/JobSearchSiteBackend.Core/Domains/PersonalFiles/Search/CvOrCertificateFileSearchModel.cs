using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;

public record CvOrCertificateFileSearchModel(
    long Id,
    string Text,
    DateTime DateTimeUpdatedUtc,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;