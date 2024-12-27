using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public class CompanySearchModel : SearchModelBase
{
    public IList<int> CountryIds { get; set; } = [];
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}