using Core.Domains.Locations.UseCases.GetLocationById;
using Core.Domains.Locations.UseCases.GetLocations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.Locations;

public static class LocationUseCasesDi
{
    public static void ConfigureLocationUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetLocationByIdHandler>();
        serviceCollection.AddScoped<GetLocationsHandler>();
    }
}