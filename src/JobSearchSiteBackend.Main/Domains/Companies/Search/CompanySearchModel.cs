using JobSearchSiteBackend.Main.Services.Search;

namespace JobSearchSiteBackend.Main.Domains.Companies.Search;

public record CompanySearchModel(
    long Id,
    long CountryId,
    string Name,
    string? Description,
    Guid VersionId,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;