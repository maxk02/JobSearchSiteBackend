using Domain.Common;

namespace Domain.Entities;

public class UserJobPermissionSet : BaseEntity
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public Guid JobId { get; set; }

    public bool CanRead { get; set; }
    public bool CanManageApplications { get; set; }
    public bool CanAttachAndEdit { get; set; }
}