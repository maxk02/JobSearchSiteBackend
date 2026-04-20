using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.Locations.Search;

public record LocationSearchModel
(
    long Id,
    long CountryId,
    string FullName,
    bool IsConcrete,
    ICollection<long> ParentIds,
    string? Description,
    string? Code
) : ISearchModelWithId;