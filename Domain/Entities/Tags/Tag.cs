using Domain.Entities.Companies;
using Domain.Entities.Jobs;
using Domain.Entities.Users;
using Domain.Shared.Entities;

namespace Domain.Entities.Tags;

public class Tag : BaseEntity
{
    public virtual Company? Company { get; set; }
    public int CompanyId { get; private set; }

    public string Name { get; private set; } = "";
    
    public string? Description { get; private set; }
    
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}