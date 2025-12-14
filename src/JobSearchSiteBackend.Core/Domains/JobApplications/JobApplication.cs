using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

public class JobApplication : IEntityWithId, IEntityWithDateTimeCreatedUtc
{
    public JobApplication(long userId, long jobId, long locationId, JobApplicationStatus status)
    {
        UserId = userId;
        JobId = jobId;
        LocationId = locationId;
        Status = status;
    }
    
    public long Id { get; private set; }
    
    public DateTime DateTimeCreatedUtc { get; private set; } = DateTime.UtcNow;
    
    public long LocationId { get; private set; }
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    public JobApplicationStatus Status { get; set; }
    
    public Location? Location { get; set; }
    public Job? Job { get; private set; }
    public UserProfile? User { get; private set; }
    
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    public ICollection<JobApplicationTag>? Tags { get; set; }
    public ICollection<UserJobApplicationBookmark>? UserJobApplicationBookmarks { get; set; }
}