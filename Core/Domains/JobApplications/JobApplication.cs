using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;

namespace Core.Domains.JobApplications;

public class JobApplication : IEntityWithId
{
    public JobApplication(long userId, long jobId, string status)
    {
        UserId = userId;
        JobId = jobId;
        Status = status;
    }
    
    public long Id { get; set; }
    public long UserId { get; private set; }
    public long JobId { get; private set; }
    public string Status { get; set; }
    
    public virtual Job? Job { get; set; }
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<PersonalFile>? PersonalFiles { get; set; }
}