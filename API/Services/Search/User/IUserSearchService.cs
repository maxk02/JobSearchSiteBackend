using API.Services.Search.Common;

namespace API.Services.Search.User;

public interface IUserSearchService : IBaseSearchService<UserSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}