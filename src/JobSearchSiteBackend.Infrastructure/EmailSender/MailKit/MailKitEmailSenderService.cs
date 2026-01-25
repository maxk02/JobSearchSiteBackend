using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace JobSearchSiteBackend.Infrastructure.EmailSender.MailKit;

public class MailKitEmailSenderService(IOptions<MySmtpSettings> settings) : IEmailSenderService
{
    public async Task SendEmailAsync(EmailToSend emailToSend,
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(emailToSend.SenderName, emailToSend.SenderEmail));
        email.To.Add(new MailboxAddress(emailToSend.To, emailToSend.To));

        if (emailToSend.Cc is not null)
        {
            email.Cc.Add(new MailboxAddress(emailToSend.Cc, emailToSend.Cc));
        }

        if (emailToSend.Bcc is not null)
        {
            email.Bcc.Add(new MailboxAddress(emailToSend.Bcc, emailToSend.Bcc));
        }

        if (emailToSend.Subject is not null)
        {
            email.Subject = emailToSend.Subject;
        }

        if (emailToSend.Body is not null)
        {
            email.Body = new TextPart(emailToSend.IsHtml ? "html" : "plain") { Text = emailToSend.Body };
        }

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(settings.Value.Server, settings.Value.Port, settings.Value.EnableSsl, cancellationToken);
        await smtp.AuthenticateAsync(settings.Value.Username, settings.Value.Password, cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}