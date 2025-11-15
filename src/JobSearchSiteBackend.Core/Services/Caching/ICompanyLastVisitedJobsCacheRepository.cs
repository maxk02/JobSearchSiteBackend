namespace JobSearchSiteBackend.Core.Services.Caching;


public interface ICompanyLastVisitedJobsCacheRepository
{
    public Task AddLastVisitedAsync(string userId, string companyId, string jobId);
    
    public Task DeleteAllLastVisitedAsync(string userId, string companyId);
    
    public Task DeleteOneLastVisitedAsync(string userId, string companyId, string jobId);
    
    public Task<ICollection<long>> GetLastVisitedAsync(string userId, string companyId, int maxLength = 15);
}