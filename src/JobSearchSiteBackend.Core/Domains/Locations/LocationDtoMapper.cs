using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public class LocationDtoMapper : Profile
{
    public LocationDtoMapper()
    {
        CreateMap<Location, LocationDto>();
    }
}