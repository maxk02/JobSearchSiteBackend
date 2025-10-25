using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles;

public class UserAvatar : IEntityWithId, IEntityWithGuid, IEntityWithUpdDelDate, IEntityWithUploadStatus
{
    public UserAvatar(long userId, string extension, long size)
    {
        UserId = userId;
        Extension = extension;
        Size = size;
    }
    
    public long Id { get; private set; }
    
    public Guid GuidIdentifier { get; private set; } = Guid.NewGuid();
    
    public DateTime DateTimeUpdatedUtc { get; set; }
    public bool IsDeleted { get; set; }
    
    public long UserId { get; private set; }
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }

    public bool IsUploadedSuccessfully { get; set; }
    
    public UserProfile? UserProfile { get; set; }
}