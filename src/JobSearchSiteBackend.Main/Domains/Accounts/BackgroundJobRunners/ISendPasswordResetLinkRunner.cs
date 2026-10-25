using JobSearchSiteBackend.Main.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;

public interface ISendPasswordResetLinkRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}