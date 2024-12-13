using API.Services.Auth.AccountStorage;
using API.Services.EmailSender;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.SendPasswordResetLinkByEmail;

public class SendPasswordResetLinkByEmailHandler(IAccountStorageService accountStorageService, IEmailSenderService emailSenderService)
{
    public async Task<Result> Handle(SendPasswordResetLinkByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var tokenGenerationResult = await accountStorageService
            .GeneratePasswordResetTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/reset-password/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendPasswordResetMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}