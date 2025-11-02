using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class CompanyAvatar : IEntityWithId, IEntityWithGuid, IEntityWithUpdDelDate, IEntityWithUploadStatus
{
    public CompanyAvatar(long? companyId, string extension, long size)
    {
        CompanyId = companyId;
        Extension = extension;
        Size = size;
    }
    
    public long Id { get; private set; }
    
    public Guid GuidIdentifier { get; private set; } = Guid.NewGuid();
    
    public DateTime DateTimeUpdatedUtc { get; set; }
    public bool IsDeleted { get; set; }
    
    public long? CompanyId { get; set; }
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }

    public bool IsUploadedSuccessfully { get; set; }
    
    public Company? Company { get; set; }
}