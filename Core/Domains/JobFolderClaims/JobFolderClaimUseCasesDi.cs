using Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.JobFolderClaims;

public static class JobFolderClaimUseCasesDi
{
    public static void ConfigureJobFolderClaimUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetJobFolderClaimIdsForUserHandler>();
        serviceCollection.AddScoped<UpdateJobFolderClaimIdsForUserHandler>();
    }
}