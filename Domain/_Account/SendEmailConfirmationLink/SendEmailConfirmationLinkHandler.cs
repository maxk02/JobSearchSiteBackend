using Domain._Shared.Services.Auth;
using Domain._Shared.Services.EmailSender;
using Shared.Result;

namespace Domain._Account.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(IAccountService accountService,
    ICurrentAccountService currentAccountService, IEmailSenderService emailSenderService)
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request,
        CancellationToken cancellationToken)
    {
        if (currentAccountService.GetEmail() is null || currentAccountService.GetEmail() != request.Email)
            return Result.Error();
        
        var tokenGenerationResult = await accountService
            .GenerateEmailConfirmationTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/confirm-email/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendEmailConfirmationMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}