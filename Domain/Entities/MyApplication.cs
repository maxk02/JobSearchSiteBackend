using Domain.Common;

namespace Domain.Entities;

public class MyApplication : BaseEntity
{
    public virtual User? User { get; set; }
    public int UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public int JobId { get; set; }
    
    public virtual IList<MyFile>? MyFiles { get; set; }
}