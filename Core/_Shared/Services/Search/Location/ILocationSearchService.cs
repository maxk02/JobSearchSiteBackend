using Core._Shared.Services.Search.Common;

namespace Core._Shared.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}