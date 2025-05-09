using JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocationById;
using JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public static class LocationUseCasesDi
{
    public static void ConfigureLocationUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetLocationByIdHandler>();
        serviceCollection.AddScoped<GetLocationsHandler>();
    }
}