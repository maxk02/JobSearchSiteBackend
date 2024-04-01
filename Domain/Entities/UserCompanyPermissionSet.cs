using Domain.Common;

namespace Domain.Entities;

public class UserCompanyPermissionSet : BaseEntity
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public Guid CompanyId { get; set; }
    
    public bool CanCreateTags { get; set; }
    public bool CanCreateJobs { get; set; }
    public bool CanReadAllJobsAndTags { get; set; }
    public bool CanManageAllApplications { get; set; }
    public bool CanManageAllJobsAndTags { get; set; }
    public bool CanManageProfile { get; set; }
}