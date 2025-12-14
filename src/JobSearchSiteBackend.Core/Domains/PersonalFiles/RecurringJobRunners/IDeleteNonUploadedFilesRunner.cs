namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobRunners;

public interface IDeleteNonUploadedFilesRunner
{
    public Task Run(int deleteOlderThanDays);
}