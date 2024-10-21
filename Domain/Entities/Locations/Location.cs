using Domain.Entities.Jobs;
using Domain.Shared.Entities;

namespace Domain.Entities.Locations;

public class Location : BaseEntity, ITreeEntity
{
    public virtual Location? Parent { get; set; }
    public int? ParentId { get; private set; }
    
    public int Level { get; private set; }

    public string Name { get; private set; } = "";
    
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    
    public virtual IList<User>? Users { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
}