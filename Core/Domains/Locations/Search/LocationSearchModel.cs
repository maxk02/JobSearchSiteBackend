using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public class LocationSearchModel : SearchModelBase
{
    public int CountryId { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Code { get; set; }
}