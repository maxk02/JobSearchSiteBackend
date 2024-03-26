using Domain.Common;

namespace Domain.Entities;

public class Location : BaseEntity, ITreeEntity
{
    public Guid? ParentId { get; set; }
    public Guid CountryId { get; set; }
    public int Level { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Code { get; set; }
}