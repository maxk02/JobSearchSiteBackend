using Hangfire;
using JobSearchSiteBackend.Main.Domains.JobApplications.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Services.EmailSender;

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