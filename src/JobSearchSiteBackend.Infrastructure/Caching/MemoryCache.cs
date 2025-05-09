using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace JobSearchSiteBackend.Infrastructure.Caching;


public class MemoryCache<TKey, TValue>(IMemoryCache memoryCache)
    : ICache<TKey, TValue> where TKey : notnull
{
    public Task<TValue?> GetAsync(TKey key)
    {
        if (memoryCache.TryGetValue(key, out TValue? value))
        {
            return Task.FromResult(value);
        }
        return Task.FromResult<TValue?>(default);
    }

    public Task SetAsync(TKey key, TValue value, CacheEntryOptions? options = null)
    {
        var cacheOptions = ToMemoryCacheEntryOptions(options);
        memoryCache.Set(key, value, cacheOptions);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TKey key)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(TKey key)
    {
        return Task.FromResult(memoryCache.TryGetValue(key, out _));
    }

    public async Task<TValue> GetOrCreateAsync(TKey key, Func<TKey, Task<TValue>> factory, CacheEntryOptions? options = null)
    {
        if (!memoryCache.TryGetValue(key, out TValue? value))
        {
            value = await factory(key);
            await SetAsync(key, value, options);
        }
        return value!;
    }

    public Task SetBatchAsync(IDictionary<TKey, TValue> items, CacheEntryOptions? options = null)
    {
        var cacheOptions = ToMemoryCacheEntryOptions(options);
        foreach (var item in items)
        {
            memoryCache.Set(item.Key, item.Value, cacheOptions);
        }
        return Task.CompletedTask;
    }

    private static MemoryCacheEntryOptions ToMemoryCacheEntryOptions(CacheEntryOptions? options)
    {
        var memoryOptions = new MemoryCacheEntryOptions();
            
        if (options != null)
        {
            memoryOptions.AbsoluteExpiration = options.AbsoluteExpiration;
            memoryOptions.SlidingExpiration = options.SlidingExpiration;
            memoryOptions.Priority = (CacheItemPriority)options.Priority;
            memoryOptions.Size = options.Size;
        }
            
        return memoryOptions;
    }
}