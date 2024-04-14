namespace Domain.Entities;

public class UserJobPermissionSet
{
    public UserJobPermissionSet(long userId, long jobId,
        bool canManageApplications, bool canAttachDetachEdit)
    {
        UserId = userId;
        JobId = jobId;
        CanManageApplications = canManageApplications;
        CanAttachDetachEdit = canAttachDetachEdit;
    }
    
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public long JobId { get; set; }
    
    public bool CanManageApplications { get; set; }
    public bool CanAttachDetachEdit { get; set; }
}