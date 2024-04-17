using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class Job : BaseEntity, IHideableEntity
{
    public virtual Company? Company { get; set; }
    public int CompanyId { get; set; }
    
    public virtual Category? Category { get; set; }
    public int CategoryId { get; set; }
    
    public string Title { get; set; } = "";
    public DateTime DateTimeExpiringUtc { get; set; }
    public SalaryRecord? SalaryRecord { get; set; }
    public string Description { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Advantages { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public bool IsHidden { get; set; }
    public bool IsExpired { get; }
    
    public virtual IList<MyApplication>? MyApplications { get; set; }
    public virtual IList<Address>? Addresses { get; set; }
    public virtual IList<ContractType>? ContractTypes { get; set; }
    public virtual IList<Tag>? Tags { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
}