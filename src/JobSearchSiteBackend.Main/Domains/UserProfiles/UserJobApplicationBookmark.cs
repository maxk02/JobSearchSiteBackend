using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.Jobs;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles;

public class UserJobApplicationBookmark : IEntityWithId, IEntityWithDateTimeCreatedUtc
{
    public UserJobApplicationBookmark(long userId, long jobApplicationId)
    {
        UserId = userId;
        JobApplicationId = jobApplicationId;
        DateTimeCreatedUtc = DateTime.UtcNow;
    }
    
    public long Id { get; private set; }
    // doesn't need sql precision, filled from backend
    public DateTime DateTimeCreatedUtc { get; private set; }
    
    public long UserId { get; private set; }
    
    public UserProfile? UserProfile { get; private set; }
    
    public long JobApplicationId { get; private set; }
    
    public JobApplication? JobApplication { get; private set; }
}