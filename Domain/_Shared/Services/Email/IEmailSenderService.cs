namespace Domain._Shared.Services.Email;

public interface IEmailSenderService
{
    Task SendEmailConfirmationMessage(string email, string confirmationLink);
    Task SendPasswordResetMessage(string email, string resetLink);
}