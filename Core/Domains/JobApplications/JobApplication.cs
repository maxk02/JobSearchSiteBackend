using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobApplications.Enums;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;

namespace Core.Domains.JobApplications;

public class JobApplication : IEntityWithId, IEntityWithDateTimeCreatedUtc
{
    public JobApplication(long userId, long jobId, JobApplicationStatus status)
    {
        UserId = userId;
        JobId = jobId;
        Status = status;
    }
    
    public long Id { get; private set; }
    
    public DateTime DateTimeCreatedUtc { get; private set; } = DateTime.UtcNow;
    
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    public JobApplicationStatus Status { get; set; }
    
    public Job? Job { get; private set; }
    public UserProfile? User { get; private set; }
    public ICollection<PersonalFile>? PersonalFiles { get; set; }
    public ICollection<JobApplicationTag>? Tags { get; set; }
}