using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.EmailSender;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLinkByEmail;

public class SendPasswordResetLinkByEmailHandler(IAccountStorageService accountStorageService,
    IEmailSenderService emailSenderService) : IRequestHandler<SendPasswordResetLinkByEmailRequest, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkByEmailRequest request, CancellationToken cancellationToken = default)
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