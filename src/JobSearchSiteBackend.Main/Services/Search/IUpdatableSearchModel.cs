namespace JobSearchSiteBackend.Core.Services.Search;

public interface IUpdatableSearchModel
{
    public Guid VersionId { get; }
    public bool IsDeleted { get; }
}