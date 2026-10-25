using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.DeleteJob;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetApplicationsForJob;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetDailyApplicationsForJobForDateRange;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJobManagementDto;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Main.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Main.Domains.Jobs;

public static class JobUseCasesDi
{
    public static void ConfigureJobUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobHandler>();
        serviceCollection.AddScoped<DeleteJobHandler>();
        serviceCollection.AddScoped<GetApplicationsForJobHandler>();
        serviceCollection.AddScoped<GetDailyApplicationsForJobForDateRangeHandler>();
        serviceCollection.AddScoped<GetDailyViewsForJobForDateRangeHandler>();
        serviceCollection.AddScoped<GetJobHandler>();
        serviceCollection.AddScoped<GetJobDataForCurrentAccountHandler>();
        serviceCollection.AddScoped<GetJobManagementDtoHandler>();
        serviceCollection.AddScoped<GetJobsHandler>();
        serviceCollection.AddScoped<UpdateJobHandler>();
    }
}