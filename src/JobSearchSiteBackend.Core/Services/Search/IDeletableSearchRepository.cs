namespace JobSearchSiteBackend.Core.Services.Search;

public interface IDeletableSearchRepository<T> where T : ISearchModelWithId, IDeletableSearchModel
{
    public Task DeleteAsync(T model, CancellationToken cancellationToken = default);
}