using Domain.Common;

namespace Domain.Entities;

public class UserCompanyPermissionSet : BaseEntity
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public long CompanyId { get; set; }
    
    public bool CanCreateTags { get; set; }
    public bool CanCreateJobs { get; set; }
    public bool CanReadAllTagsAndHiddenJobs { get; set; }
    public bool CanManageAllApplications { get; set; }
    public bool CanEditAllJobsAndTags { get; set; }
    public bool CanEditProfile { get; set; }
}