using Core.Domains.JobContractTypes.UseCases.GetJobContractTypesByCountryId;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.JobContractTypes;

public static class JobContractTypeUseCasesDi
{
    public static void ConfigureJobContractTypeUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetJobContractTypesByCountryIdHandler>();
    }
}