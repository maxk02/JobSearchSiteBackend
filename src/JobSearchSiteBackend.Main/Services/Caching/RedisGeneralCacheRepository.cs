using JobSearchSiteBackend.Core.Services.Caching;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Caching;

public class RedisGeneralCacheRepository : IGeneralCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisGeneralCacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = connectionMultiplexer.GetDatabase();
    }

    // public async Task<string?> GetAsync(string key)
    // {
    //     var value = await _database.StringGetAsync(key);
    //     return value.IsNullOrEmpty ? null : value.ToString();
    // }
    //
    // public async Task SetAsync(string key, string value, TimeSpan? storageDuration = null)
    // {
    //     await _database.StringSetAsync(key, value, storageDuration);
    // }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    // public async Task SetBatchAsync(IDictionary<string, string> items, TimeSpan? storageDuration = null)
    // {
    //     if (items.Count == 0)
    //         return;
    //
    //     var tasks = new List<Task>();
    //
    //     foreach (var item in items)
    //     {
    //         tasks.Add(_database.StringSetAsync(item.Key, item.Value, storageDuration));
    //     }
    //
    //     await Task.WhenAll(tasks);
    // }
}