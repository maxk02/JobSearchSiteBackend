using Domain._Shared.Services.Search.Common;

namespace Domain._Shared.Services.Search.User;

public interface IUserSearchService : IBaseSearchService<UserSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}