using Shared.Result;

namespace Core.Services.EmailSender;

public interface IEmailSenderService
{
    public Task<Result> SendEmailConfirmationMessageAsync(string email, string confirmationLink,
        CancellationToken cancellationToken = default);
    public Task<Result> SendPasswordResetMessageAsync(string email, string resetLink,
        CancellationToken cancellationToken = default);
}