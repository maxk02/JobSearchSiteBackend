using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public class JobSearchModel : SearchModelBase
{
    public long CompanyId { get; set; }
    public long CategoryId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<string> Responsibilities { get; set; } = [];
    public ICollection<string> Requirements { get; set; } = [];
    public ICollection<string> Advantages { get; set; } = [];
}