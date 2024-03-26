using Domain.Common;

namespace Domain.Entities;

public class Tag : BaseEntity, ITreeEntity
{
    public Guid? ParentId { get; set; }
    public Guid CompanyId { get; set; }
    public int Level { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}