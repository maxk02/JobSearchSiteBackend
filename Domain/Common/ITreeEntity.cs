namespace Domain.Common;

public interface ITreeEntity
{
    public long? ParentId { get; set; }
    public int Level { get; set; }
}