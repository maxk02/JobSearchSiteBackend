using JobSearchSiteBackend.Core.Domains.Countries.UseCases.GetAllCountries;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Countries;

public static class CountryUseCasesDi
{
    public static void ConfigureCountryUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetAllCountriesHandler>();
    }
}