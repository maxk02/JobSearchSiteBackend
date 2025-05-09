using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

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