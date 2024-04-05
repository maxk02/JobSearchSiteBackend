using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class Job : BaseEntity, IHideableEntity
{
    // nullable typ do wsparcia lazy loading
    public virtual Company? Company { get; set; }
    public long CompanyId { get; set; }
    
    public virtual Category? Category { get; set; }
    public long CategoryId { get; set; }
    
    public string Title { get; set; } = "";
    public DateTimeOffset? DateTimeExpiring { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public string Description { get; set; } = "";
    public List<string> Responsibilities { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Benefits { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public bool IsHidden { get; set; } = false;
    
    public virtual IList<Application>? Applications { get; set; }
    public virtual IList<Address>? Addresses { get; set; }
    public virtual IList<ContractType>? JobContractTypes { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    public virtual IList<UserJobPermissionSet>? UserJobPermissionSets { get; set; }
}