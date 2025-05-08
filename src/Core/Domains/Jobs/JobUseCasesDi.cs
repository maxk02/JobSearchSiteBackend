using Core.Domains.Jobs.UseCases.AddJob;
using Core.Domains.Jobs.UseCases.DeleteJob;
using Core.Domains.Jobs.UseCases.GetJobById;
using Core.Domains.Jobs.UseCases.GetJobs;
using Core.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.Jobs;

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