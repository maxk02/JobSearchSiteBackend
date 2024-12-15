using Core.Services.Search.Common;

namespace Core.Services.Search.User;

public interface IUserSearchService : IBaseSearchService<UserSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}