namespace Core.Domains._Shared.Search;

public interface ISearchRepository<T> where T : SearchModelBase
{
    Task AddAsync(T searchModel);
    Task UpdateAsync(T searchModel);
    Task DeleteAsync(long id);
    Task<ICollection<long>> Search(string query);
}