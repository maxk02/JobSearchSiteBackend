namespace Domain.Shared.Entities;

public interface ITreeEntity
{
    public int? ParentId { get; }
    public int Level { get; }
}