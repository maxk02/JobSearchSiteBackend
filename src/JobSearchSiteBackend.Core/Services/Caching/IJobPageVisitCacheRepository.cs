namespace JobSearchSiteBackend.Core.Services.Caching;


public interface IJobPageVisitCacheRepository
{
    public Task IncrementJobVisitsCounterAsync(string companyId, string jobId);
    public Task<long> GetJobViewsForDateAsync(string companyId, string jobId, DateTime date);
    public Task<long> GetCumulatedCompanyViewsForDateAsync(string companyId, DateTime date);
    public Task<Dictionary<DateTime, long>> GetJobViewsForDateRangeAsync(string companyId, string jobId,
        DateTime startDate, DateTime endDate);

    public Task<long> GetTotalCompanyViewsForDateRangeAsync(string companyId, DateTime startDate,
        DateTime endDate);
}