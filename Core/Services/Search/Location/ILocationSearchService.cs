using Core.Services.Search.Common;

namespace Core.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}