using JobSearchSiteBackend.Main.Services.Search;

namespace JobSearchSiteBackend.Main.Domains.Locations.Search;

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