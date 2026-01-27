using Hangfire;
using JobSearchSiteBackend.Core.Domains.Companies.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.BackgroundJobsRunners;

public class SendCompanyEmployeeInvitationEmailRunner(IBackgroundJobClient backgroundJobClient) : ISendCompanyEmployeeInvitationEmailRunner
{
    public async Task RunAsync(EmailToSend emailToSend)
    {
        backgroundJobClient.Enqueue<IEmailSenderService>(emailSenderService =>
            emailSenderService.SendEmailAsync(emailToSend, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}