using Core.Domains._Shared.Search;

namespace Core.Domains.UserProfiles.Search;

public interface IUserSearchRepository : ISearchRepository<UserSearchModel>
{
    Task<IList<long>> SearchByCountryId(string query, long countryId);
}