using Hangfire;
using JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.BackgroundJobsRunners;

public class ResendEmailConfirmationLinkRunner(IBackgroundJobClient backgroundJobClient) : IResendEmailConfirmationLinkRunner
{
    public async Task RunAsync(EmailToSend emailToSend)
    {
        backgroundJobClient.Enqueue<IEmailSenderService>(emailSenderService =>
            emailSenderService.SendEmailAsync(emailToSend, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}