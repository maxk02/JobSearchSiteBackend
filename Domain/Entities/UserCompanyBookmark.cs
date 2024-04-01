using Domain.Common;

namespace Domain.Entities;

public class UserCompanyBookmark : BaseEntity
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Company? Company { get; set; }
    public Guid CompanyId { get; set; }

}