using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public static class JobUseCasesDi
{
    public static void ConfigureJobUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobHandler>();
        serviceCollection.AddScoped<DeleteJobHandler>();
        serviceCollection.AddScoped<GetApplicationsForJobHandler>();
        serviceCollection.AddScoped<GetJobHandler>();
        serviceCollection.AddScoped<GetJobManagementDtoHandler>();
        serviceCollection.AddScoped<GetJobsHandler>();
        serviceCollection.AddScoped<UpdateJobHandler>();
    }
}