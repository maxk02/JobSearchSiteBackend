using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class Job : BaseEntity, IHideableEntity
{
    public virtual Company? Company { get; set; }
    public long CompanyId { get; set; }
    
    public virtual Category? Category { get; set; }
    public long CategoryId { get; set; }
    
    public string Title { get; set; } = "";
    public DateTimeOffset? DateTimeExpiring { get; set; }
    public SalaryRecord? SalaryInfo { get; set; }
    public string Description { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Advantages { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public bool IsHidden { get; set; } = false;
    
    public virtual IList<MyApplication>? Applications { get; set; }
    public virtual IList<Address>? Addresses { get; set; }
    public virtual IList<ContractType>? ContractTypes { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    public virtual IList<UserJobPermissionSet>? UserJobPermissionSets { get; set; }
}