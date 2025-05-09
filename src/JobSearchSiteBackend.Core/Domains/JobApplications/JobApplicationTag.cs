using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

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