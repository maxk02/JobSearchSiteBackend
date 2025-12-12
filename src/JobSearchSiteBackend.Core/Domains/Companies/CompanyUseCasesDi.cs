using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedFolders;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedFolders;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.TopUpCompanyBalance;
// using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanies;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public static class CompanyUseCasesDi
{
    public static void ConfigureCompanyUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddCompanyHandler>();
        serviceCollection.AddScoped<AddCompanyEmployeeHandler>();
        serviceCollection.AddScoped<DeleteCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyBalanceHandler>();
        serviceCollection.AddScoped<GetCompanyEmployeesHandler>();
        serviceCollection.AddScoped<GetCompanyJobsHandler>();
        serviceCollection.AddScoped<GetCompanyLastVisitedFoldersHandler>();
        serviceCollection.AddScoped<GetCompanyLastVisitedJobsHandler>();
        serviceCollection.AddScoped<GetCompanyManagementNavbarDtoHandler>();
        serviceCollection.AddScoped<GetCompanySharedFoldersRootHandler>();
        serviceCollection.AddScoped<RemoveCompanyEmployeeHandler>();
        serviceCollection.AddScoped<RemoveCompanyLastVisitedFoldersHandler>();
        serviceCollection.AddScoped<RemoveCompanyLastVisitedJobsHandler>();
        serviceCollection.AddScoped<SearchCompanySharedFoldersHandler>();
        serviceCollection.AddScoped<SearchCompanySharedJobsHandler>();
        serviceCollection.AddScoped<TopUpCompanyBalanceHandler>();
        serviceCollection.AddScoped<UpdateCompanyHandler>();
        serviceCollection.AddScoped<UploadCompanyAvatarHandler>();
    }
}