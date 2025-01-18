using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Companies;
using Core.Domains.JobFolderClaims;
using Core.Domains.Jobs;
using Ardalis.Result;

namespace Core.Domains.JobFolders;

public class JobFolder : IEntityWithId, IHierarchicalEntity<JobFolder, JobFolderRelation>
{
    public JobFolder(long companyId, string? name, string? description)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
    }
    
    public long Id { get; private set; }
    
    public long CompanyId { get; private set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public Company? Company { get; set; }
    
    public ICollection<JobFolderRelation>? RelationsWhereThisIsDescendant { get; set; }
    public ICollection<JobFolderRelation>? RelationsWhereThisIsAncestor { get; set; }
    
    public ICollection<Job>? Jobs { get; set; }
    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
}