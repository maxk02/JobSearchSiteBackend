using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Companies.BackgroundJobRunners;

public interface ISendCompanyEmployeeInvitationEmailRunner
{
    public Task RunAsync(EmailToSend emailToSend);
}