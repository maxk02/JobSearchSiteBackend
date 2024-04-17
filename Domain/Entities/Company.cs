using Domain.Common;

namespace Domain.Entities;

public class Company : BaseEntity, IHideableEntity
{
    public string Name { get; set; } = "";
    public DateOnly? DateFounded { get; set; }
    public string? Description { get; set; }
    
    public bool IsHidden { get; set; }
    
    public virtual IList<Tag>? Tags { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
}