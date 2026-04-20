using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.Companies.Search;

public record CompanySearchModel(
    long Id,
    long CountryId,
    string Name,
    string? Description,
    Guid VersionId,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;