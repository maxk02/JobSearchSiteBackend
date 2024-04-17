using Application.Services.Search.Common;

namespace Application.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<IList<int>> SearchForCountryId(string query, int countryId);
}