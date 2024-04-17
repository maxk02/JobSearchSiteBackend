namespace Domain.Common;

public interface ITreeEntity
{
    public int? ParentId { get; set; }
    public int Level { get; set; }
}