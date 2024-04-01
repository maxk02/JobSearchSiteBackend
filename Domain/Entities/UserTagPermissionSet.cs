namespace Domain.Entities;

public class UserTagPermissionSet
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Tag? Tag { get; set; }
    public Guid TagId { get; set; }

    public bool CanReadJobs { get; set; }
    public bool CanManageApplications { get; set; }
    public bool CanEditJobs { get; set; }
    public bool CanEditTag { get; set; }
}