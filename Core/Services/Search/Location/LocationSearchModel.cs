using Core.Services.Search.Common;

namespace Core.Services.Search.Location;

public class LocationSearchModel : BaseSearchModel
{
    public int CountryId { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Code { get; set; }
}