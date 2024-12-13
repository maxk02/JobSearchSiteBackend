using API.Services.Search.Common;

namespace API.Services.Search.Company;

public class CompanySearchModel : BaseSearchModel
{
    public IList<int> CountryIds { get; set; } = [];
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}