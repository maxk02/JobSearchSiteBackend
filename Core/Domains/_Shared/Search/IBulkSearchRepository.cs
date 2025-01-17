namespace Core.Domains._Shared.Search;

public interface IBulkSearchRepository<T> where T : ISearchModelWithId
{
    public Task AddOrUpdateManyAsync(ICollection<T> searchModels, CancellationToken cancellationToken = default);
    public Task DeleteManyAsync(ICollection<long> searchModelIds, CancellationToken cancellationToken = default);
}