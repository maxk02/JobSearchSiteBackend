using Application.Services.Search.Common;

namespace Application.Services.Search.Job;

public class JobSearchModel : BaseSearchModel
{
    public long CompanyId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Benefits { get; set; } = [];
}