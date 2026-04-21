using System.Globalization;
using JobSearchSiteBackend.Core.Services.Caching;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Caching;

public class RedisJobPageVisitCacheRepository : IJobPageVisitCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    
    public RedisJobPageVisitCacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = connectionMultiplexer.GetDatabase();
    }


    public async Task IncrementJobVisitsCounterAsync(string companyId, string jobId)
    {
        var nowUtc = DateTime.UtcNow;

        var dateString = nowUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var key1 = $"company:{companyId}:job:{jobId}:views:{dateString}";
        var key2 = $"company:{companyId}:cumulated-job-views:{dateString}";
        
        const string script = @"
            local a = redis.call('INCR', KEYS[1])
            local b = redis.call('INCR', KEYS[2])
            return {a, b}
        ";

        var result = await _database.ScriptEvaluateAsync(script, [key1, key2]);
    }
    
    public async Task<long> GetJobViewsForDateAsync(string companyId, string jobId, DateTime date)
    {
        var dateString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var key = $"company:{companyId}:job:{jobId}:views:{dateString}";
        
        var value = await _database.StringGetAsync(key);
        
        return value.HasValue ? (long)value : 0;
    }
    
    public async Task<long> GetCumulatedCompanyViewsForDateAsync(string companyId, DateTime date)
    {
        var dateString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var key = $"company:{companyId}:cumulated-job-views:{dateString}";
        
        var value = await _database.StringGetAsync(key);
        
        return value.HasValue ? (long)value : 0;
    }
    
    public async Task<Dictionary<DateTime, long>> GetJobViewsForDateRangeAsync(string companyId, string jobId, DateTime startDate, DateTime endDate)
    {
        var dates = new List<DateTime>();
        for (var dt = startDate.Date; dt <= endDate.Date; dt = dt.AddDays(1))
        {
            dates.Add(dt);
        }
        
        var keys = dates.Select(d => (RedisKey)$"company:{companyId}:job:{jobId}:views:{d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}").ToArray();

        // MGET
        var values = await _database.StringGetAsync(keys);

        var result = new Dictionary<DateTime, long>();
        for (var i = 0; i < dates.Count; i++)
        {
            result[dates[i]] = values[i].HasValue ? (long)values[i] : 0;
        }

        return result;
    }
    
    public async Task<long> GetTotalCompanyViewsForDateRangeAsync(string companyId, DateTime startDate, DateTime endDate)
    {
        var dates = new List<DateTime>();
        for (var dt = startDate.Date; dt <= endDate.Date; dt = dt.AddDays(1))
        {
            dates.Add(dt);
        }
        
        var keys = dates.Select(d => (RedisKey)$"company:{companyId}:cumulated-job-views:{d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}").ToArray();
        
        var values = await _database.StringGetAsync(keys);

        long totalViews = 0;
        
        foreach (var value in values)
        {
            if (value.HasValue)
            {
                totalViews += (long)value;
            }
        }

        return totalViews;
    }
}