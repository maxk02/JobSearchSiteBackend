using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims;

public static class CompanyClaimUseCasesDi
{
    public static void ConfigureCompanyClaimUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetCompanyClaimIdsForUserHandler>();
        serviceCollection.AddScoped<UpdateCompanyClaimIdsForUserHandler>();
    }
}