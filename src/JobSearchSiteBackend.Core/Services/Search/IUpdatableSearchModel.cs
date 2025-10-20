namespace JobSearchSiteBackend.Core.Services.Search;

public interface IUpdatableSearchModel : IDeletableSearchModel
{
    public DateTime DateTimeUpdatedUtc { get; }
}