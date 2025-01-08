using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobFolders;
using Core.Domains.UserProfiles;

namespace Core.Domains.JobFolderClaims;

public class UserJobFolderClaim : IEntityWithId
{
    public UserJobFolderClaim(long userId, long folderId, long permissionId)
    {
        UserId = userId;
        FolderId = folderId;
        PermissionId = permissionId;
    }
    
    public long Id { get; set; }
    public long UserId { get; private set; }
    public long FolderId { get; private set; }
    public long PermissionId { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual JobFolder? JobFolder { get; set; }
    public virtual JobFolderClaim? JobFolderPermission { get; set; }
}