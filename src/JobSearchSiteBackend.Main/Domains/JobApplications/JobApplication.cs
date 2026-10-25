using JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Main.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Main.Domains.Jobs;
using JobSearchSiteBackend.Main.Domains.Locations;
using JobSearchSiteBackend.Main.Domains.PersonalFiles;
using JobSearchSiteBackend.Main.Domains.UserProfiles;

namespace JobSearchSiteBackend.Main.Domains.JobApplications;

public class JobApplication : IEntityWithId, IEntityWithDateTimeCreatedUtc, IEntityWithSearchSync
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
    
    public Guid VersionId { get; set; } = Guid.NewGuid();
    
    public Guid? VersionIdSyncedWithSearch { get; set; } = null;
    
    public bool IsDeleted { get; set; }
    
    public long LocationId { get; private set; }
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    public JobApplicationStatus Status { get; set; }
    
    public Location? Location { get; set; }
    public Job? Job { get; private set; }
    public UserProfile? User { get; private set; }
    
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    public ICollection<JobApplicationTag>? Tags { get; set; }
}