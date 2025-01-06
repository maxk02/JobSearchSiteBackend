using Core.Domains._Shared.Search;

namespace Core.Domains.Cvs.Search;

public interface ICvSearchRepository : ISearchRepository<CvSearchModel>
{
    public Task<ICollection<long>> SearchFromAppliedToJobAsync(string query, long jobId, CancellationToken cancellationToken = default);
    public Task AddAppliedToJobIdAsync(long userId, long jobId, CancellationToken cancellationToken = default);
    public Task RemoveAppliedToJobIdAsync(long userId, long jobId, CancellationToken cancellationToken = default);
}