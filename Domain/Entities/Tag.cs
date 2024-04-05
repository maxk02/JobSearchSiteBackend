using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class Tag : BaseEntity
{
    public virtual Company? Company { get; set; }
    public long CompanyId { get; set; }
    
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}