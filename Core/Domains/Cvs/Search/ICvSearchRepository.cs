using Core.Domains._Shared.Search;

namespace Core.Domains.Cvs.Search;

public interface ICvSearchRepository : ISearchRepository<CvSearchModel>
{
    public Task<ICollection<long>> SearchFromAppliedToJobAsync(string query, long jobId);
    public Task AddAppliedToJobIdAsync(long userId, long jobId);
    public Task RemoveAppliedToJobIdAsync(long userId, long jobId);
}