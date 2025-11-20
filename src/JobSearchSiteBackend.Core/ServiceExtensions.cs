using System.Reflection;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core;

public static class ServiceExtensions
{
    public static void ConfigureCoreAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
    
    public static void ConfigureUseCases(this IServiceCollection services)
    {
        services.ConfigureAccountUseCases();
        services.ConfigureCompanyUseCases();
        services.ConfigureCompanyClaimUseCases();
        services.ConfigureCountryUseCases();
        services.ConfigureJobApplicationUseCases();
        services.ConfigureJobFolderClaimUseCases();
        services.ConfigureJobFolderUseCases();
        services.ConfigureJobUseCases();
        services.ConfigureLocationUseCases();
        services.ConfigurePersonalFileUseCases();
        services.ConfigureUserProfileUseCases();
    }

    public static void ConfigureEmailRenderers(this IServiceCollection services)
    {
        services.AddScoped<StandardEmailRenderer>();
    }
}