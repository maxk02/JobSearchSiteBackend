namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobRunners;

public interface ISyncTextFilesWithSearchRunner
{
    public Task Run();
}