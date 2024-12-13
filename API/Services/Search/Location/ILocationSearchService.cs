using API.Services.Search.Common;

namespace API.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}