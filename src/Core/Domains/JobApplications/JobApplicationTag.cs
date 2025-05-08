using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobApplications.Enums;
using Core.Domains.Jobs;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;

namespace Core.Domains.JobApplications;

public class JobApplicationTag : IEntityWithId
{
    public JobApplicationTag(long jobApplicationId, string tag)
    {
        JobApplicationId = jobApplicationId;
        Tag = tag;
    }
    
    public long Id { get; private set; }
    public long JobApplicationId { get; private set; }
    
    public string Tag { get; private set; }
    
    
    public JobApplication? JobApplication { get; private set; }
}