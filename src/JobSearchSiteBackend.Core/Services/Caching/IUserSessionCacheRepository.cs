namespace JobSearchSiteBackend.Core.Services.Caching;


public interface IUserSessionCacheRepository
{
    public Task AddSessionAsync(string userId, string tokenId, DateTime expirationUtc);
    
    public Task DeleteAllSessionsAsync(string userId);
    
    public Task DeleteAllSessionsExceptCurrentAsync(string userId, string tokenId);
    
    public Task DeleteSessionAsync(string userId, string tokenId);
    
    public Task<DateTime?> GetSessionExpirationUtcAsync(string userId, string tokenId);
    
    public Task ProlongSessionAsync(string userId, string tokenId, DateTime expirationUtc);
}