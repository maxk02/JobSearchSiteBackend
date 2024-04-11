using Domain.Common;

namespace Domain.Entities;

public class UserJobPermissionSet : BaseEntity
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public long JobId { get; set; }

    public bool CanReadHidden { get; set; }
    public bool CanManageApplications { get; set; }
    public bool CanAttachAndEdit { get; set; }
}