using Domain.Common;

namespace Domain.Entities;

public class UserTagPermissionSet : BaseEntity
{
    public virtual User? User { get; set; }
    public int UserId { get; set; }
    
    public virtual Tag? Tag { get; set; }
    public int TagId { get; set; }
    
    public bool CanManageApplications { get; set; }
    public bool CanCreateEditDeleteJobs { get; set; }
    public bool IsTagAdmin { get; set; }
    public bool IsTagOwner { get; set; }
}