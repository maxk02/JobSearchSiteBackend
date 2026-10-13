namespace JobSearchSiteBackend.Core.Services.EmailSender;

public interface IEmailSenderService
{
    Task SendEmailAsync(EmailToSend emailToSend, CancellationToken cancellationToken = default);
}