using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Locations.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

namespace JobSearchSiteBackend.API.Controllers.Locations;

public class LocationsDtosMapper : Profile
{
    public LocationsDtosMapper()
    {
        CreateMap<GetLocationsResult, GetLocationsResponse>();
    }
}