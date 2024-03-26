using Domain.Common;

namespace Domain.Entities;

public class Job : BaseEntity, IHideableEntity
{
    public Guid CompanyId { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = "";
    public DateTimeOffset? DateTimeExpiring { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public string? Description { get; set; }
    public List<string> Responsibilities { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Benefits { get; set; } = [];
    public bool IsHidden { get; set; } = false;
}