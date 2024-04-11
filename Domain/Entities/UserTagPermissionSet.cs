namespace Domain.Entities;

public class UserTagPermissionSet
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Tag? Tag { get; set; }
    public long TagId { get; set; }

    public bool CanReadHiddenJobs { get; set; }
    public bool CanManageApplications { get; set; }
    public bool CanEditJobs { get; set; }
    public bool CanEditTag { get; set; }
}