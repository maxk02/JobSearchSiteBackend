namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.RecurringJobRunners;

public interface IDeleteNonUploadedFilesRunner
{
    public Task Run(int deleteOlderThanDays);
}