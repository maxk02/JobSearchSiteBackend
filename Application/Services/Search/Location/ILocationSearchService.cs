using Application.Services.Search.Common;

namespace Application.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<List<int>> SearchForCountryId(string query, Guid countryId);
}