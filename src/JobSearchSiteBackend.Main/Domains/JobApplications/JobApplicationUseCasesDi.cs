using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplicationTag;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.RemoveJobApplicationTag;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

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