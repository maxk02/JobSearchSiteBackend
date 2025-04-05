namespace Core.Services.Caching;


public interface ICache<TKey, TValue> where TKey : notnull
{
    Task<TValue?> GetAsync(TKey key);
    
    Task SetAsync(TKey key, TValue value, CacheEntryOptions? options = null);
    
    Task RemoveAsync(TKey key);
    
    Task<bool> ExistsAsync(TKey key);
    
    Task<TValue> GetOrCreateAsync(TKey key, Func<TKey, Task<TValue>> factory, CacheEntryOptions? options = null);
    
    Task SetBatchAsync(IDictionary<TKey, TValue> items, CacheEntryOptions? options = null);
}