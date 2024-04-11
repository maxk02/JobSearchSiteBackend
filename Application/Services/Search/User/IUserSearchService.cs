using Application.Services.Search.Common;

namespace Application.Services.Search.User;

public interface IUserSearchService : IBaseSearchService<UserSearchModel>
{
    Task<IList<long>> SearchForCountryId(string query, long countryId);
}