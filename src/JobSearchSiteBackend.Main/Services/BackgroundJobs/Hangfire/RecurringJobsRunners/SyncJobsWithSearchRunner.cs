using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;

public class SyncJobsWithSearchRunner(MainDataContext dbContext,
    IJobSearchRepository jobSearchRepository) : ISyncJobsWithSearchRunner
{
    public async Task Run()
    {
        var recordsToSyncQuery = dbContext.Jobs
            .Where(j => j.VersionIdSyncedWithSearch == null
                        || j.VersionIdSyncedWithSearch != j.VersionId);
        
        var count = await recordsToSyncQuery.CountAsync();
        
        if (count == 0)
            return;
        
        var jobSearchModels = await recordsToSyncQuery.Select(job => new JobSearchModel(
            job.Id,
            job.Company!.CountryId,
            job.CategoryId,
            job.CompanyId,
            job.Title,
            job.Description,
            job.IsPublic,
            job.DateTimePublishedUtc,
            job.DateTimeExpiringUtc,
            job.Responsibilities ?? new List<string>(),
            job.Requirements ?? new List<string>(),
            job.NiceToHaves ?? new List<string>(),
            job.EmploymentOptions!.Select(eo => eo.Id).ToList(),
            job.JobContractTypes!.Select(jct => jct.Id).ToList(),
            job.Locations!.Select(l => l.Id).ToList(),
            job.Locations!.SelectMany(l => l.RelationsWhereThisIsDescendant!).Select(rel => rel.AncestorId).Distinct().ToList(),
            job.Locations!.SelectMany(l => l.RelationsWhereThisIsAncestor!).Select(rel => rel.DescendantId).Distinct().ToList(),
            job.VersionId,
            job.IsDeleted
        )).ToListAsync();

        await jobSearchRepository.UpsertMultipleAsync(jobSearchModels);
        
        var idsToUpdate = jobSearchModels.Select(x => x.Id).ToList();

        // EXECUTE UPDATE:
        // This generates a direct SQL UPDATE statement.
        // It bypasses the ChangeTracker, effectively skipping all SaveChanges interceptors.
        await dbContext.Jobs
            .Where(j => idsToUpdate.Contains(j.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(j => j.VersionIdSyncedWithSearch, j => j.VersionId));
    }
}