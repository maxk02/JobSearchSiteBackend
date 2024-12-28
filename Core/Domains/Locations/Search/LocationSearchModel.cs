using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public class LocationSearchModel : SearchModelBase
{
    public long CountryId { get; set; }
    public required string Name { get; set; }
    public ICollection<string> Subdivisions { get; set; } = [];
    public string? Description { get; set; }
    public string? Code { get; set; }
}