using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.EmailSending;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public class SendPasswordResetLinkHandler(UserManager<MyIdentityUser> userManager,
    IEmailSendingService emailSendingService) : IRequestHandler<SendPasswordResetLinkRequest, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.NotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var link = "https://example.com/reset-password/" + token; //todo

        var emailSendingResult = await emailSendingService
            .SendPasswordResetMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}