using AutoMapper;

namespace Core.Domains.Locations.Dtos;

public class LocationDtoMapper : Profile
{
    public LocationDtoMapper()
    {
        CreateMap<Location, LocationDto>();
    }
}