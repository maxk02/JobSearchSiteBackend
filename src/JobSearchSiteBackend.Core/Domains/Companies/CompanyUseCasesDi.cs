using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployeeInvitation;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalanceTransactions;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetJobApplicationTags;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.TopUpCompanyBalance;
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
        serviceCollection.AddScoped<AddCompanyEmployeeInvitationHandler>();
        serviceCollection.AddScoped<DeleteCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyBalanceHandler>();
        serviceCollection.AddScoped<GetCompanyBalanceTransactionsHandler>();
        serviceCollection.AddScoped<GetCompanyEmployeesHandler>();
        serviceCollection.AddScoped<GetCompanyJobManagementCardDtosHandler>();
        serviceCollection.AddScoped<GetCompanyJobsHandler>();
        serviceCollection.AddScoped<GetCompanyLastVisitedJobsHandler>();
        serviceCollection.AddScoped<GetCompanyManagementNavbarDtoHandler>();
        serviceCollection.AddScoped<GetJobApplicationTagsHandler>();
        serviceCollection.AddScoped<RemoveCompanyEmployeeHandler>();
        serviceCollection.AddScoped<RemoveCompanyLastVisitedJobsHandler>();
        serviceCollection.AddScoped<SearchCompanySharedJobsHandler>();
        serviceCollection.AddScoped<TopUpCompanyBalanceHandler>();
        serviceCollection.AddScoped<UpdateCompanyHandler>();
        serviceCollection.AddScoped<UploadCompanyAvatarHandler>();
    }
}