using Hangfire;
using JobSearchSiteBackend.Core.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.BackgroundJobRunners.Hangfire;

public class SendApplicationStatusUpdatedEmailRunner(IBackgroundJobClient backgroundJobClient) : ISendApplicationStatusUpdatedEmailRunner
{
    public async Task RunAsync(EmailToSend emailToSend)
    {
        backgroundJobClient.Enqueue<IEmailSenderService>(emailSenderService =>
            emailSenderService.SendEmailAsync(emailToSend, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}