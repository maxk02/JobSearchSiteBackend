using Core._Shared.Services.Search.Common;

namespace Core._Shared.Services.Search.Company;

public class CompanySearchModel : BaseSearchModel
{
    public IList<int> CountryIds { get; set; } = [];
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}