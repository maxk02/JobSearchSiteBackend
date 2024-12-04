using Domain._Shared.Services.Search.Common;

namespace Domain._Shared.Services.Search.Job;

public class JobSearchModel : BaseSearchModel
{
    public int CompanyId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Benefits { get; set; } = [];
}