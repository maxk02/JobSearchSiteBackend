using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobById;
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
        serviceCollection.AddScoped<GetJobByIdHandler>();
        serviceCollection.AddScoped<GetJobsHandler>();
        serviceCollection.AddScoped<UpdateJobHandler>();
    }
}