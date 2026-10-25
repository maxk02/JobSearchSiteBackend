namespace JobSearchSiteBackend.Main.Services.EmailSender;

public interface IEmailSenderService
{
    Task SendEmailAsync(EmailToSend emailToSend, CancellationToken cancellationToken = default);
}