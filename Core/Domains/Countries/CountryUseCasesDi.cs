using Core.Domains.Countries.UseCases.GetAllCountries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.Countries;

public static class CountryUseCasesDi
{
    public static void ConfigureCountryUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetAllCountriesHandler>();
    }
}