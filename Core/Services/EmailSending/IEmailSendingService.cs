using Shared.Result;

namespace Core.Services.EmailSending;

public interface IEmailSendingService
{
    public Task<Result> SendEmailConfirmationMessageAsync(string email, string confirmationLink,
        CancellationToken cancellationToken = default);
    public Task<Result> SendPasswordResetMessageAsync(string email, string resetLink,
        CancellationToken cancellationToken = default);
}