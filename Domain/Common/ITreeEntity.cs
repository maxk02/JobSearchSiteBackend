namespace Domain.Common;

public interface ITreeEntity
{
    public Guid? ParentId { get; set; }
    public int Level { get; set; }
}