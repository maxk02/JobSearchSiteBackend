using Domain.Common;

namespace Domain.Entities;

public class UserJobBookmark : BaseEntity
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public long JobId { get; set; }
}