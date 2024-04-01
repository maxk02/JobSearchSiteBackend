using Domain.Common;

namespace Domain.Entities;

public class UserJobBookmark : BaseEntity
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public Guid JobId { get; set; }
}