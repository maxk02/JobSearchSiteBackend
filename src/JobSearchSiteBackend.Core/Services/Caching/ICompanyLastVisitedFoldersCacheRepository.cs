namespace JobSearchSiteBackend.Core.Services.Caching;


public interface ICompanyLastVisitedFoldersCacheRepository
{
    public Task AddLastVisitedAsync(string userId, string companyId, string folderId);
    
    public Task DeleteAllLastVisitedAsync(string userId, string companyId);
    
    public Task DeleteOneLastVisitedAsync(string userId, string companyId, string folderId);
    
    public Task<ICollection<long>> GetLastVisitedAsync(string userId, string companyId, int maxLength = 15);
}