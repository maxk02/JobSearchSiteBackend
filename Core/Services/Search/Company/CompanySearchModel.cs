using Core.Services.Search.Common;

namespace Core.Services.Search.Company;

public class CompanySearchModel : BaseSearchModel
{
    public IList<int> CountryIds { get; set; } = [];
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}