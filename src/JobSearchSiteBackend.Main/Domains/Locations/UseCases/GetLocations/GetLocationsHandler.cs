using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Locations.Dtos;
using JobSearchSiteBackend.Main.Domains.Locations.Search;
using JobSearchSiteBackend.Main.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Locations.UseCases.GetLocations;

public class GetLocationsHandler(
    ILocationSearchRepository locationSearchRepository)
    : IRequestHandler<GetLocationsQuery, Result<GetLocationsResult>>
{
    public async Task<Result<GetLocationsResult>> Handle(GetLocationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var hits = await locationSearchRepository
            .SearchFromCountryIdAsync(query.CountryId, query.Query, query.IsConcrete, query.Size, cancellationToken);

        var locationDtos = hits
            .Select(h => new LocationDto(h.Id, h.CountryId,h.FullName, h.Description, h.Code))
            .ToList();

        var response = new GetLocationsResult(locationDtos);

        return response;
    }
}