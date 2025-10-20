namespace JobSearchSiteBackend.Core.Services.Search;

public interface IUpdatableSearchRepository<T> where T : ISearchModelWithId, IUpdatableSearchModel
{
    public Task UpdateAsync(T model, CancellationToken cancellationToken = default);
}