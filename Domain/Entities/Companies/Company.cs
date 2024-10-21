using Domain.Entities.Jobs;
using Domain.Entities.Tags;
using Domain.Entities.Users;
using Domain.Shared.Entities;

namespace Domain.Entities.Companies;

public class Company : BaseEntity, IHideableEntity
{
    public string Name { get; private set; }
    
    public DateOnly? DateFounded { get; set; }
    public string? Description { get; set; }
    
    public required bool IsHidden { get; set; }
    
    public virtual IList<Tag>? Tags { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
}