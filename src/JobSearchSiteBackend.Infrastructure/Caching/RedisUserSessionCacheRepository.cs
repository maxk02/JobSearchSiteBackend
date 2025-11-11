using System.Globalization;
using JobSearchSiteBackend.Core.Services.Caching;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Caching;

public class RedisUserSessionCacheRepository : IUserSessionCacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonCommands _json;
    
    public RedisUserSessionCacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = connectionMultiplexer.GetDatabase();
        _json = _database.JSON();
    }

    public async Task AddSessionAsync(string userId, string tokenId, DateTime expirationUtc)
    {
        var nowUtc = DateTime.UtcNow;
        
        if (expirationUtc < nowUtc) return;
        
        await _database.StringSetAsync(
            $"users:{userId}:sessions:{tokenId}:expiresUtc",
            expirationUtc.ToString("o"),
            expirationUtc.Subtract(nowUtc)
            );
    }

    public async Task DeleteAllSessionsAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return;
        
        var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints()[0]);
        
        var keys = server.Keys(pattern: $"users:{userId}:sessions*").ToArray();
        
        if (keys.Length == 0)
            return;
        
        await _database.KeyDeleteAsync(keys);
    }
    
    public async Task DeleteAllSessionsExceptCurrentAsync(string userId, string tokenId)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tokenId))
            return;
        
        var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints()[0]);
        
        var keys = server
            .Keys(pattern: $"users:{userId}:sessions*")
            .Where(k => k.ToString() != $"users:{userId}:sessions:{tokenId}:expiresUtc")
            .ToArray();
        
        if (keys.Length == 0)
            return;
        
        await _database.KeyDeleteAsync(keys);
    }

    public async Task DeleteSessionAsync(string userId, string tokenId)
    {
        await _database.KeyDeleteAsync($"users:{userId}:sessions:{tokenId}:expiresUtc");
    }

    public async Task<DateTime?> GetSessionExpirationUtcAsync(string userId, string tokenId)
    {
        var value = await _database.StringGetAsync($"users:{userId}:sessions:{tokenId}:expiresUtc");

        if (value.IsNullOrEmpty)
            return null;
        
        var parsed = DateTime.TryParse(value.ToString(), null,
            DateTimeStyles.RoundtripKind, out var expirationUtc);
        
        return parsed ? expirationUtc : null;
    }

    public async Task ProlongSessionAsync(string userId, string tokenId, DateTime expirationUtc)
    {
        var nowUtc = DateTime.UtcNow;

        if (expirationUtc < nowUtc)
            return;
        
        var value = await _database.StringGetAsync($"users:{userId}:sessions:{tokenId}:expiresUtc");

        if (value.IsNullOrEmpty)
            return;
        
        await _database.StringSetAsync(
            $"users:{userId}:sessions:{tokenId}:expiresUtc",
            expirationUtc.ToString(CultureInfo.InvariantCulture),
            expirationUtc.Subtract(nowUtc)
        );
    }
}