using Core.Domains.JobApplications.UseCases.AddJobApplication;
using Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;
using Core.Domains.JobApplications.UseCases.UpdateJobApplication;
using Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.JobApplications;

public static class JobApplicationUseCasesDi
{
    public static void ConfigureJobApplicationUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobApplicationHandler>();
        serviceCollection.AddScoped<DeleteJobApplicationHandler>();
        serviceCollection.AddScoped<GetApplicationsForJobIdHandler>();
        serviceCollection.AddScoped<UpdateJobApplicationHandler>();
        serviceCollection.AddScoped<UpdateJobApplicationFilesHandler>();
    }
}