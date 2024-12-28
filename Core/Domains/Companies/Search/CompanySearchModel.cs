using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public class CompanySearchModel : SearchModelBase
{
    public required long CountryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}