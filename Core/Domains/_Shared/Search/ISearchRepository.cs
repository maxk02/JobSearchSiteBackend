namespace Core.Domains._Shared.Search;

public interface ISearchRepository<T> where T : ISearchModel
{
    public Task AddAsync(T searchModel, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T searchModel, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    public Task<ICollection<long>> SearchAllAsync(string query, CancellationToken cancellationToken = default);
}