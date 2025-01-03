using Core.Domains._Shared.Search;
using Core.Domains.Cvs.Search;

namespace Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<CvSearchModel>
{
    public Task<ICollection<long>> SearchFromAppliedToJobAsync(string query, long jobId);
    public Task AddAppliedToJobIdAsync(ICollection<long> fileIds, long jobId);
    public Task RemoveAppliedToJobIdAsync(ICollection<long> fileIds, long jobId);
}