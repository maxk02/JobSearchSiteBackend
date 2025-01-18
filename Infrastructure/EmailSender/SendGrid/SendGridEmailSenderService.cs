using Core.Services.EmailSender;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.EmailSender.SendGrid;

public class SendGridEmailSenderService(string sendGridApiKey, string senderEmail, string senderName) : IEmailSenderService
{
    public async Task SendEmailAsync(string to, string subject, string content,
        CancellationToken cancellationToken = default)
    {
        var client = new SendGridClient(sendGridApiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(senderEmail, senderName),
            Subject = subject,
            HtmlContent = content
        };
        msg.AddTo(new EmailAddress(to));
        var response = await client.SendEmailAsync(msg, cancellationToken);
        
        if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            throw new HttpRequestException();
        }
    }
}