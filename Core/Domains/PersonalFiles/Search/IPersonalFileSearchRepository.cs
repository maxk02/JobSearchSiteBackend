using Core.Domains._Shared.Search;
using Core.Domains.Cvs.Search;

namespace Core.Domains.PersonalFiles.Search;

public interface IPersonalFileSearchRepository : ISearchRepository<PersonalFileSearchModel>
{
    public Task<ICollection<long>> SearchFromAppliedToJobAsync(string query, long jobId, CancellationToken cancellationToken = default);
    public Task AddAppliedToJobIdAsync(ICollection<long> fileIds, long jobId, CancellationToken cancellationToken = default);
    public Task RemoveAppliedToJobIdAsync(ICollection<long> fileIds, long jobId, CancellationToken cancellationToken = default);
}