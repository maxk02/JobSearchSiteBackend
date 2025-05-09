namespace JobSearchSiteBackend.Core.Services.Search;

public interface ISearchModelWithDeletionDateTime
{
    public DateTime? DeletionDateTimeUtc { get; }
}