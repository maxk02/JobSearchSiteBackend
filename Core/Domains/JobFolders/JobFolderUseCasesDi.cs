using Core.Domains.JobFolders.UseCases.AddJobFolder;
using Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using Core.Domains.JobFolders.UseCases.GetChildJobsAndFolders;
using Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.JobFolders;

public static class JobFolderUseCasesDi
{
    public static void ConfigureJobFolderUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobFolderHandler>();
        serviceCollection.AddScoped<DeleteJobFolderHandler>();
        serviceCollection.AddScoped<GetChildJobsAndFoldersHandler>();
        serviceCollection.AddScoped<UpdateJobFolderHandler>();
    }
}