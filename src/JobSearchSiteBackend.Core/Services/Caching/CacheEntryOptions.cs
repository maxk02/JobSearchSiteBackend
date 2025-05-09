namespace JobSearchSiteBackend.Core.Services.Caching;

public class CacheEntryOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    
    public TimeSpan? SlidingExpiration { get; set; }
    
    public CachePriority Priority { get; set; } = CachePriority.Normal;
    
    public long? Size { get; set; }
}