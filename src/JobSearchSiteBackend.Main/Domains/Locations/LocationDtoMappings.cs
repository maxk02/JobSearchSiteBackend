using JobSearchSiteBackend.Main.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Locations;

public static class LocationDtoMappings
{
    public static LocationDto ToLocationDto(this Location location)
    {
        var locationDto = new LocationDto(location.Id, location.CountryId,
            location.FullName, location.DescriptionPl, location.Code);
        
        return locationDto;
    }
}