using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Search;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

public class GetLocationsHandler(
    ILocationSearchRepository locationSearchRepository)
    : IRequestHandler<GetLocationsQuery, Result<GetLocationsResult>>
{
    public async Task<Result<GetLocationsResult>> Handle(GetLocationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var hits = await locationSearchRepository
            .SearchFromCountryIdAsync(query.CountryId, query.Query, query.Size, cancellationToken);

        var locationDtos = hits
            .Select(h => new LocationDto(h.Id, h.CountryId,h.FullName, h.Description, h.Code))
            .ToList();

        var response = new GetLocationsResult(locationDtos);

        return response;
    }
}