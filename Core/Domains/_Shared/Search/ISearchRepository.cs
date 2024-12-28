namespace Core.Domains._Shared.Search;

public interface ISearchRepository<T> where T : SearchModelBase
{
    public Task AddAsync(T searchModel);
    public Task UpdateAsync(T searchModel);
    public Task DeleteAsync(long id);
    public Task<ICollection<long>> SearchAllAsync(string query);
}