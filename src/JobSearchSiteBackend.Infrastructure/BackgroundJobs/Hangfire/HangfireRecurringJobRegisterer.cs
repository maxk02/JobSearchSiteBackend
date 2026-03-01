using Hangfire;
using JobSearchSiteBackend.Core.Domains.Companies.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobRunners;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire;

public class HangfireRecurringJobRegisterer(IRecurringJobManager recurringJobManager) : IRecurringJobRegisterer
{
    public Task RegisterJobsAsync()
    {
        recurringJobManager.AddOrUpdate<IClearCompanyAvatarsRunner>("clear-company-avatars",
            clearCompanyAvatarsRunner => clearCompanyAvatarsRunner.Run(), "0 0 * * *");
        
        recurringJobManager.AddOrUpdate<ISyncJobsWithSearchRunner>("sql-search-sync-jobs", 
            syncJobsWithSearchRunner => syncJobsWithSearchRunner.Run(), $"*/1 * * * *");
        
        recurringJobManager.AddOrUpdate<IDeleteNonUploadedFilesRunner>("sql-delete-non-uploaded-cv-or-cert-files", 
            deleteNonUploadedFilesRunner => deleteNonUploadedFilesRunner.Run(1), $"0 0 * * *");
        
        recurringJobManager.AddOrUpdate<ISyncJobApplicationsWithSearchRunner>("sql-search-sync-job-applications",
            syncTextFilesWithSearchRunner => syncTextFilesWithSearchRunner.Run(), $"*/1 * * * *");

        return Task.CompletedTask;
    }
}