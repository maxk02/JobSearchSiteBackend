namespace JobSearchSiteBackend.Core.Services.Search;

public interface IUpdatableSearchModel
{
    public DateTime DateTimeUpdatedUtc { get; }
    public bool IsDeleted { get; }
}