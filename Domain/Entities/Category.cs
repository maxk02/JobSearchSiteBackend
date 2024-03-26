using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity, ITreeEntity
{
    public Guid? ParentId { get; set; }
    public int Level { get; set; }
    public string Name { get; set; } = "";
}