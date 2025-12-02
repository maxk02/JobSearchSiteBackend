using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Jobs;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles;

public class UserJobBookmark : IEntityWithId, IEntityWithDateTimeCreatedUtc
{
    public UserJobBookmark(long userId, long jobId)
    {
        UserId = userId;
        JobId = jobId;
        DateTimeCreatedUtc = DateTime.UtcNow;
    }
    
    public long Id { get; private set; }
    // doesn't need sql precision, filled from backend
    public DateTime DateTimeCreatedUtc { get; private set; }
    
    public long UserId { get; private set; }
    
    public UserProfile? UserProfile { get; private set; }
    
    public long JobId { get; private set; }
    
    public Job? Job { get; private set; }
}