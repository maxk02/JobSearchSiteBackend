using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.Jobs;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolders;

public class JobFolder : IEntityWithId, IHierarchicalEntity<JobFolder, JobFolderRelation>
{
    public JobFolder(long companyId, string? name, string? description)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
    }
    
    public long Id { get; set; }
    
    public long CompanyId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public Company? Company { get; set; }
    
    public ICollection<JobFolderRelation>? RelationsWhereThisIsDescendant { get; set; }
    public ICollection<JobFolderRelation>? RelationsWhereThisIsAncestor { get; set; }
    
    public ICollection<Job>? Jobs { get; set; }
    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
}