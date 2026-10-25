using JobSearchSiteBackend.Main.Domains.Companies.UseCases.AcceptCompanyEmployeeInvitation;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyBalance;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyBalanceTransactions;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyEmployeeInvitation;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyEmployees;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobs;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetJobApplicationTags;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.RemoveCompanyEmployee;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.SendCompanyEmployeeInvitation;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.TopUpCompanyBalance;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.UpdateCompany;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.UploadCompanyAvatar;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Main.Domains.Companies;

public static class CompanyUseCasesDi
{
    public static void ConfigureCompanyUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddCompanyHandler>();
        serviceCollection.AddScoped<AcceptCompanyEmployeeInvitationHandler>();
        serviceCollection.AddScoped<SendCompanyEmployeeInvitationHandler>();
        serviceCollection.AddScoped<DeleteCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyHandler>();
        serviceCollection.AddScoped<GetCompanyBalanceHandler>();
        serviceCollection.AddScoped<GetCompanyBalanceTransactionsHandler>();
        serviceCollection.AddScoped<GetCompanyEmployeeInvitationHandler>();
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