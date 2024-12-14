using Core._Shared.Services.Auth;
using Core._Shared.Services.EmailSender;
using Shared.Result;

namespace Core._Account.SendPasswordResetLinkByEmail;

public class SendPasswordResetLinkByEmailHandler(IAccountService accountService, IEmailSenderService emailSenderService)
{
    public async Task<Result> Handle(SendPasswordResetLinkByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var tokenGenerationResult = await accountService
            .GeneratePasswordResetTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/reset-password/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendPasswordResetMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}