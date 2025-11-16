using System.Globalization;
using JobSearchSiteBackend.Core.Services.Caching;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Caching;

public class RedisPageVisitCacheRepository : IPageVisitCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    
    public RedisPageVisitCacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = connectionMultiplexer.GetDatabase();
    }


    public async Task IncrementJobVisitsCounterAsync(string companyId, string jobId)
    {
        var nowUtc = DateTime.UtcNow;

        var dateString = nowUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var hourString = nowUtc.Hour.ToString();

        var key1 = $"company:{companyId}:job:{jobId}:views:{dateString}:{hourString}";
        var key2 = $"company:{companyId}:cumulated-job-views:{dateString}:{hourString}";
        
        const string script = @"
            local a = redis.call('INCR', KEYS[1])
            local b = redis.call('INCR', KEYS[2])
            return {a, b}
        ";

        var result = await _database.ScriptEvaluateAsync(script, [key1, key2]);

    }
}