namespace Core.Domains._Shared.Search;

public interface ISearchModelWithDeletionDateTime
{
    public DateTime? DeletionDateTimeUtc { get; }
}