using Application.Services.Search.Common;

namespace Application.Services.Search.Job;

public class JobSearchModel : BaseSearchModel
{
    public long CompanyId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Responsibilities { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Benefits { get; set; } = [];
}