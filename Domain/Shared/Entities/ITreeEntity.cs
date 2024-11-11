namespace Domain.Shared.Entities;

public interface ITreeEntity
{
    public long? ParentId { get; }
    public int Level { get; }
}