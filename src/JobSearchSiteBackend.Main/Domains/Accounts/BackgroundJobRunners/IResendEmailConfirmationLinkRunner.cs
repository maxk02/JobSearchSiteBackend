using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;

public interface IResendEmailConfirmationLinkRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}