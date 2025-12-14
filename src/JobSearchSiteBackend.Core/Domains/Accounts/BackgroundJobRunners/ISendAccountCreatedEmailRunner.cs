using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;

public interface ISendAccountCreatedEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}