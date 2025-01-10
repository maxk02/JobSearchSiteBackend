using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobFolders;
using Core.Domains.UserProfiles;

namespace Core.Domains.JobFolderClaims;

public class UserJobFolderClaim : IEntityWithId
{
    public UserJobFolderClaim(long userId, long folderId, long claimId)
    {
        UserId = userId;
        FolderId = folderId;
        ClaimId = claimId;
    }
    
    public long Id { get; set; }
    public long UserId { get; private set; }
    public long FolderId { get; private set; }
    public long ClaimId { get; private set; }
    
    public UserProfile? User { get; set; }
    public JobFolder? JobFolder { get; set; }
    public JobFolderClaim? JobFolderClaim { get; set; }
}