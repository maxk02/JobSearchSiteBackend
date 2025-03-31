using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Locations.Dtos;
using Core.Domains.Locations.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Locations.UseCases.GetLocations;

public class GetLocationsHandler(
    MainDataContext context,
    ILocationSearchRepository locationSearchRepository,
    IMapper mapper)
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

        var locationDtos = await query
            .ProjectTo<LocationDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var response = new GetLocationsResponse(locationDtos);

        return response;
    }
}