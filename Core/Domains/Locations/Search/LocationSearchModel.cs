using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public record LocationSearchModel
(
    long Id,
    long CountryId,
    string Name,
    ICollection<string> Subdivisions,
    string? Description,
    string? Code
) : ISearchModelWithId;