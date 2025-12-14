using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;

public interface ISendPasswordResetLinkRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}