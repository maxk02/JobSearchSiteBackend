using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Core.Services.EmailSending;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    IEmailSendingService emailSendingService) : IRequestHandler<SendEmailConfirmationLinkRequest, Result>
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        
        if (user is null)
            return Result.NotFound();
        
        if (user.Email is null || user.Email != request.Email)
            return Result.Forbidden();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var link = "https://example.com/confirm-email/" + token; //todo

        var emailSendingResult = await emailSendingService
            .SendEmailConfirmationMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}