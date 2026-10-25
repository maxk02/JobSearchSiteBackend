using JobSearchSiteBackend.Main.Domains.Locations.UseCases.GetLocations;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Main.Domains.Locations;

public static class LocationUseCasesDi
{
    public static void ConfigureLocationUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetLocationsHandler>();
    }
}