using Core.Domains._Shared.Search;

namespace Core.Domains.UserProfiles.Search;

public interface ICvSearchRepository : ISearchRepository<CvSearchModel>
{
    public Task<ICollection<long>> SearchFromAppliedToJobAsync(string query, long jobId);
}