namespace JobSearchSiteBackend.Core.Services.Caching;


public interface IPageVisitCacheRepository
{
    public Task IncrementJobVisitsCounterAsync(string companyId, string jobId);
}