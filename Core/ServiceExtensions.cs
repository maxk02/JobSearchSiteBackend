using System.Reflection;
using Core.Domains.Accounts;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.Countries;
using Core.Domains.JobApplications;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.Locations;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

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
}