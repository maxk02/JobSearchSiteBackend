using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public interface IJobSearchRepository : ISearchRepository<JobSearchModel>
{
    Task<ICollection<long>> SearchByCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}