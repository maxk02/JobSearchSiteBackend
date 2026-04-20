namespace JobSearchSiteBackend.Core.Services.Caching;


public interface IJobPageVisitCacheRepository
{
    public Task IncrementJobVisitsCounterAsync(string companyId, string jobId);
}