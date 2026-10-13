namespace JobSearchSiteBackend.Core.Domains.JobApplications;

public class JobApplicationTag
{
    public JobApplicationTag(long jobApplicationId, string tag)
    {
        JobApplicationId = jobApplicationId;
        Tag = tag;
    }
    
    public long JobApplicationId { get; private set; }
    
    public string Tag { get; private set; }
    
    
    public JobApplication? JobApplication { get; private set; }
}