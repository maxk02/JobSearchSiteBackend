using Hangfire;
using JobSearchSiteBackend.Main.Domains.Companies.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Companies.BackgroundJobRunners.Hangfire;

public class SendCompanyEmployeeInvitationEmailRunner(IBackgroundJobClient backgroundJobClient) : ISendCompanyEmployeeInvitationEmailRunner
{
    public async Task RunAsync(EmailToSend emailToSend)
    {
        backgroundJobClient.Enqueue<IEmailSenderService>(emailSenderService =>
            emailSenderService.SendEmailAsync(emailToSend, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}