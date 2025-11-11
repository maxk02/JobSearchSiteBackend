namespace JobSearchSiteBackend.Core.Services.Caching;


public interface IGeneralCacheRepository
{
    // Task<T?> GetAsync(string key);
    
    // Task SetAsync(string key, T value, TimeSpan? storageDuration = null);
    
    Task RemoveAsync(string key);
    
    Task<bool> ExistsAsync(string key);
    
    // Task SetBatchAsync(IDictionary<string, T> items, TimeSpan? storageDuration = null);
}