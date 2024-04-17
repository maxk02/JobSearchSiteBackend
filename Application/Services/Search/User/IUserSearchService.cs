using Application.Services.Search.Common;

namespace Application.Services.Search.User;

public interface IUserSearchService : IBaseSearchService<UserSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}