using Domain.Common;

namespace Domain.Entities;

public class Address : BaseEntity
{
    public virtual Location? Location { get; set; }
    public int LocationId { get; set; }
    
    public string Line1 { get; set; } = "";
    public string? Line2 { get; set; }
    public string? ZipCode { get; set; }
    
    public virtual IList<Job>? Jobs { get; set; }
}