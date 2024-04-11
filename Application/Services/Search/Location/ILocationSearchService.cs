using Application.Services.Search.Common;

namespace Application.Services.Search.Location;

public interface ILocationSearchService : IBaseSearchService<LocationSearchModel>
{
    Task<IList<long>> SearchForCountryId(string query, long countryId);
}