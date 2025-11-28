using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.AddJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetFolderJobs;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.JobFolders;

public static class JobFolderUseCasesDi
{
    public static void ConfigureJobFolderUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobFolderHandler>();
        serviceCollection.AddScoped<DeleteJobFolderHandler>();
        serviceCollection.AddScoped<GetChildFoldersHandler>();
        serviceCollection.AddScoped<GetFolderJobsHandler>();
        serviceCollection.AddScoped<UpdateJobFolderHandler>();
    }
}