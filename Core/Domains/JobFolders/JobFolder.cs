using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Companies;
using Core.Domains.JobFolderClaims;
using Core.Domains.Jobs;
using Shared.Result;

namespace Core.Domains.JobFolders;

public class JobFolder : IEntityWithId, IHierarchicalEntity<JobFolder, JobFolderClosure>
{
    public JobFolder(long? companyId, string? name, string? description)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
    }
    
    public long Id { get; set; }
    
    public long? CompanyId { get; private set; }
    
    public string? Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public virtual Company? Company { get; set; }
    
    public ICollection<JobFolderClosure>? Ancestors { get; set; }
    public ICollection<JobFolderClosure>? Descendants { get; set; }
    
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
}