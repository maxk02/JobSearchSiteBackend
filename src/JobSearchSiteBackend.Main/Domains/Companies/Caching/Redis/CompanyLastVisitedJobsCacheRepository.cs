using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Main.Domains.Companies.Caching.Redis;

public class CompanyLastVisitedJobsCacheRepository : ICompanyLastVisitedJobsCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonCommands _json;
    
    public CompanyLastVisitedJobsCacheRepository(IConnectionMultiplexer connectionMultiplexer)
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
            .Select(v => long.TryParse((string?)v, out var num) ? num : 0L)
            .ToList();

        return longIds;
    }
}