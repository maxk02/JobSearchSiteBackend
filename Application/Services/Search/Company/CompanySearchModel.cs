using Application.Services.Search.Common;

namespace Application.Services.Search.Company;

public class CompanySearchModel : BaseSearchModel
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}