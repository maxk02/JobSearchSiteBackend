using Application.Services.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.EmailSender;

public class SendGridEmailSenderService : IEmailSenderService
{
    private readonly string _sendGridApiKey;
    private readonly string _senderEmail;

    public SendGridEmailSenderService(string sendGridApiKey, string senderEmail)
    {
        _sendGridApiKey = sendGridApiKey;
        _senderEmail = senderEmail;
    }
    
    public async Task SendEmailConfirmationMessage(string email, string confirmationLink)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_senderEmail, "znajdzprace.pl"),
            Subject = "Aktywacja konta znajdzprace.pl",
            HtmlContent = $"Witamy, dziękujemy za rejestrację w serwisie znadjzprace.pl." +
                          $" Prosimy o podtwierdzenie adresu e-mail poprzez kliknięcie w link poniżej w celu" +
                          $" aktywacji konta:<br><a href='{confirmationLink}'>link</a>"
        };
        msg.AddTo(new EmailAddress(email));
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            // throw new Exception($"Failed to send email message. Status code: {response.StatusCode}");
            // log
        }
    }

    public async Task SendPasswordResetMessage(string email, string resetLink)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_senderEmail, "znajdzprace.pl"),
            Subject = "Resetowanie hasła znajdzprace.pl",
            HtmlContent = $"Witamy, przed chwilą otrzymaliśmy żądanie resetowania hasła w serwisie znadjzprace.pl." +
                          $" Prosimy o kliknięcie w link poniżej w celu resetowania hasła lub o" +
                          $" ignorowanie tej wiadomości w razie niewysyłania tego żądania." +
                          $"<br><br>Link: <a href='{resetLink}'>link</a>"
        };
        msg.AddTo(new EmailAddress(email));
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            // throw new Exception($"Failed to send email message. Status code: {response.StatusCode}");
            // log
        }
    }
}