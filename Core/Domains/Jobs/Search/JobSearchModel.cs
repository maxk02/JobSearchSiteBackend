using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public class JobSearchModel : SearchModelBase
{
    public int CompanyId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Benefits { get; set; } = [];
}