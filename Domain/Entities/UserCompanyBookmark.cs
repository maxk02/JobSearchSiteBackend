using Domain.Common;

namespace Domain.Entities;

public class UserCompanyBookmark : BaseEntity
{
    public virtual User? User { get; set; }
    public int UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public int CompanyId { get; set; }
}