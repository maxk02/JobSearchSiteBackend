using Hangfire;
using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners.Hangfire;

public class SendPasswordResetLinkRunner(IBackgroundJobClient backgroundJobClient) : ISendPasswordResetLinkRunner
{
    public async Task RunAsync(EmailToSend emailToSend)
    {
        backgroundJobClient.Enqueue<IEmailSenderService>(emailSenderService =>
            emailSenderService.SendEmailAsync(emailToSend, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}