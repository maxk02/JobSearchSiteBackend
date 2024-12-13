using API.Services.Search.Common;

namespace API.Services.Search.Location;

public class LocationSearchModel : BaseSearchModel
{
    public int CountryId { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Code { get; set; }
}