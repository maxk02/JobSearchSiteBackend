using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;

public interface IResendEmailConfirmationLinkRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}