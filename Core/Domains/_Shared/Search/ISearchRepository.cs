namespace Core.Domains._Shared.Search;

public interface ISearchRepository<T> where T : ISearchModelWithId
{
    public string IndexName { get; }
    public Task CreateIndexAsync(CancellationToken cancellationToken = default);
    public Task<bool> CheckIndexExistenceAsync(CancellationToken cancellationToken = default);
    public Task<ICollection<long>> SearchFromIdsAsync(ICollection<long> ids, string query,
        CancellationToken cancellationToken = default);
    public Task<ICollection<long>> SearchFromAllAsync(string query, CancellationToken cancellationToken = default);
}