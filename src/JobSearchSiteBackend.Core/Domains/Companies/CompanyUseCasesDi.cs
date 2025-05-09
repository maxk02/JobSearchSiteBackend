using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanies;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyById;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public static class CompanyUseCasesDi
{
    public static void ConfigureCompanyUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddCompanyHandler>();
        serviceCollection.AddScoped<DeleteCompanyHandler>();
        serviceCollection.AddScoped<GetCompaniesHandler>();
        serviceCollection.AddScoped<GetCompanyByIdHandler>();
        serviceCollection.AddScoped<UpdateCompanyHandler>();
    }
}