using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class Location : BaseEntity, ITreeEntity
{
    public virtual Location? Parent { get; set; }
    public int? ParentId { get; set; }
    
    public int Level { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Code { get; set; }
    
    public virtual IList<User>? Users { get; set; }
    public virtual IList<Address>? Addresses { get; set; }
}