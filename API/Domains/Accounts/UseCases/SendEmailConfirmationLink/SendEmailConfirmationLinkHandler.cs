using API.Services.Auth.AccountStorage;
using API.Services.Auth.CurrentAccount;
using API.Services.EmailSender;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(IAccountStorageService accountStorageService,
    ICurrentAccountService currentAccountService, IEmailSenderService emailSenderService)
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request,
        CancellationToken cancellationToken)
    {
        if (currentAccountService.GetEmail() is null || currentAccountService.GetEmail() != request.Email)
            return Result.Error();
        
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