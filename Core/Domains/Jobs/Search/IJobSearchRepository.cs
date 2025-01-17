using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public interface IJobSearchRepository : ISearchRepository<JobSearchModel>, IConcurrentSearchRepository<JobSearchModel>
{
    Task<ICollection<long>> SearchFromCountriesAndCategoriesAsync(ICollection<long> countryIds,
        ICollection<long> categoryIds, string query, CancellationToken cancellationToken = default);
}