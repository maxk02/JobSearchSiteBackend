using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.Locations.Dtos;

public class LocationDtoMapper : Profile
{
    public LocationDtoMapper()
    {
        CreateMap<Location, LocationDto>();
    }
}