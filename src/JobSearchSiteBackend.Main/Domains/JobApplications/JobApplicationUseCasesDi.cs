using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.AddJobApplicationTag;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.DeleteJobApplication;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.RemoveJobApplicationTag;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Main.Domains.JobApplications;

public static class JobApplicationUseCasesDi
{
    public static void ConfigureJobApplicationUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobApplicationHandler>();
        serviceCollection.AddScoped<AddJobApplicationTagHandler>();
        serviceCollection.AddScoped<DeleteJobApplicationHandler>();
        serviceCollection.AddScoped<GetFileDownloadLinkFromJobApplicationHandler>();
        serviceCollection.AddScoped<RemoveJobApplicationTagHandler>();
        serviceCollection.AddScoped<UpdateJobApplicationFilesHandler>();
        serviceCollection.AddScoped<UpdateJobApplicationStatusHandler>();
    }
}