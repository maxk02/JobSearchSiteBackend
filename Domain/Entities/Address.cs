using Domain.Common;

namespace Domain.Entities;

public class Address : BaseEntity
{
    public virtual Location? Location { get; set; }
    public Guid LocationId { get; set; }
    
    public virtual Job? Job { get; set; }
    public Guid JobId { get; set; }
    
    public string Line1 { get; set; } = "";
    public string? Line2 { get; set; }
    public string? ZipCode { get; set; }
}