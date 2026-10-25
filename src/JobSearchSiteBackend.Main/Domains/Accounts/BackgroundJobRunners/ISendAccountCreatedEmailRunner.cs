using JobSearchSiteBackend.Main.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;

public interface ISendAccountCreatedEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}