using JobSearchSiteBackend.Main.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.BackgroundJobRunners;

public interface ISendApplicationStatusUpdatedEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}