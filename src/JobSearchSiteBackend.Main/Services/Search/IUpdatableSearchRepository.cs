namespace JobSearchSiteBackend.Core.Services.Search;

public interface IUpdatableSearchRepository<T> where T : ISearchModelWithId, IUpdatableSearchModel
{
    public Task UpsertMultipleAsync(ICollection<T> searchModels, CancellationToken cancellationToken = default);
}