using JobSearchSiteBackend.Core.Services.EmailSender;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using JobSearchSiteBackend.Shared.MyAppSettings;

namespace JobSearchSiteBackend.Infrastructure.EmailSender.MailKit;

public class MailKitEmailSenderService(IOptions<MySmtpSettings> settings) : IEmailSenderService
{
    public async Task SendEmailAsync(string to, string subject, string content,
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(settings.Value.SenderName, settings.Value.SenderEmail));
        email.To.Add(new MailboxAddress(to, to));
        email.Subject = subject;

        email.Body = new TextPart("html") { Text = content };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(settings.Value.Server, settings.Value.Port, settings.Value.EnableSsl, cancellationToken);
        await smtp.AuthenticateAsync(settings.Value.Username, settings.Value.Password, cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}