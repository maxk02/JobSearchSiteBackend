using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public static class LocationDtoMappings
{
    public static LocationDto ToLocationDto(this Location location)
    {
        var locationDto = new LocationDto(location.Id, location.CountryId,
            location.FullName, location.DescriptionPl, location.Code);
        
        return locationDto;
    }
}