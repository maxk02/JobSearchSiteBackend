using System.Globalization;
using JobSearchSiteBackend.Core.Services.Caching;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Caching;

public class RedisCompanyLastVisitedJobsCacheRepository : ICompanyLastVisitedJobsCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonCommands _json;
    
    public RedisCompanyLastVisitedJobsCacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = connectionMultiplexer.GetDatabase();
        _json = _database.JSON();
    }
    
    
    public async Task AddLastVisitedAsync(string userId, string companyId, string jobId)
    {
        const string script = """

                              redis.call('LREM', KEYS[1], 0, ARGV[1])
                              redis.call('LPUSH', KEYS[1], ARGV[1])
                              redis.call('LTRIM', KEYS[1], 0, 14)
                              return redis.call('LRANGE', KEYS[1], 0, -1)

                              """;

        var key = $"companies:{companyId}:users:{userId}:recent-jobs";
        var result = await _database.ScriptEvaluateAsync(script, [key], [jobId]);
    }

    public async Task DeleteAllLastVisitedAsync(string userId, string companyId)
    {
        await _database.KeyDeleteAsync($"companies:{companyId}:users:{userId}:recent-jobs");
    }

    public async Task DeleteOneLastVisitedAsync(string userId, string companyId, string jobId)
    {
        await _database.ListRemoveAsync($"companies:{companyId}:users:{userId}:recent-jobs", jobId, 1);
    }
    
    public async Task<ICollection<long>> GetLastVisitedAsync(string userId, string companyId, int maxLength = 15)
    {
        var listObjects = await _database
            .ListRangeAsync($"companies:{companyId}:users:{userId}:recent-jobs", 0, maxLength);

        if (listObjects.Length == 0)
            return [];

        var longIds = listObjects
            .Select(v => long.TryParse(v, out var num) ? num : 0L)
            .ToList();

        return longIds;
    }
}