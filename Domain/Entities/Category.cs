using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity, ITreeEntity
{
    public long? ParentId { get; set; }
    
    public int Level { get; set; }
    public string Name { get; set; } = "";
    
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<User>? Users { get; set; }
}