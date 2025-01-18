using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Locations.Dtos;
using Core.Domains.Locations.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Locations.UseCases.GetLocations;

public class GetLocationsHandler(MainDataContext context, ILocationSearchRepository locationSearchRepository)
    : IRequestHandler<GetLocationsRequest, Result<GetLocationsResponse>>
{
    public async Task<Result<GetLocationsResponse>> Handle(GetLocationsRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await locationSearchRepository
            .SearchFromCountryIdAsync(request.CountryId, request.Query, cancellationToken);

        var query = context.Locations
            .Where(l => hitIds.Contains(l.Id))
            .Take(10);

        var locations = await query.ToListAsync(cancellationToken);

        var locationDtos = locations
            .Select(l => new LocationDto(l.Id, l.CountryId, l.Name, l.Subdivisions, l.Description, l.Code))
            .ToList();

        var response = new GetLocationsResponse(locationDtos);

        return response;
    }
}