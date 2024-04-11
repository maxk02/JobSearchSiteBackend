using Domain.Common;

namespace Domain.Entities;

public class UserCompanyBookmark : BaseEntity
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public long CompanyId { get; set; }

}