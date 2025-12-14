using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.BackgroundJobRunners;

public interface ISendApplicationStatusUpdatedEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}