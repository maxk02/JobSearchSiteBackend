using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Core.Services.EmailSender;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(ICurrentAccountService currentAccountService,
    IAccountStorageService accountStorageService,
    IEmailSenderService emailSenderService) : IRequestHandler<SendEmailConfirmationLinkRequest, Result>
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request, CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetEmailOrThrow() != request.Email)
            return Result.Forbidden();
        
        var tokenGenerationResult = await accountStorageService
            .GenerateEmailConfirmationTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/confirm-email/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendEmailConfirmationMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}