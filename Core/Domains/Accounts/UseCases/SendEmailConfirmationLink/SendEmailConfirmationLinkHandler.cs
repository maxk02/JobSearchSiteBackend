using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Core.Services.Auth;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    UserManager<MyIdentityUser> userManager,
    IEmailSenderService emailSenderService) : IRequestHandler<SendEmailConfirmationLinkRequest, Result>
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request, CancellationToken cancellationToken = default)
    {
        if (jwtCurrentAccountService.GetEmailOrThrow() != request.Email)
            return Result.Forbidden();
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.NotFound();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var link = "https://example.com/confirm-email/" + token; //todo

        var emailSendingResult = await emailSenderService
            .SendEmailConfirmationMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}