using Domain.Common;

namespace Domain.Entities;

public class UserCompanyPermissionSet : BaseEntity
{
    public virtual User? User { get; set; }
    public int UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public int CompanyId { get; set; }
    
    public bool CanCreateTags { get; set; }
    public bool IsCompanyAdmin { get; set; }
    public bool IsCompanyOwner { get; set; }
}