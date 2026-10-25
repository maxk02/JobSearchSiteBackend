using JobSearchSiteBackend.Main.Services.EmailSender;

namespace JobSearchSiteBackend.Main.Domains.Companies.BackgroundJobRunners;

public interface ISendCompanyEmployeeInvitationEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}