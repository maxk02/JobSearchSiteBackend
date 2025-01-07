using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLinkByEmail;

public class SendPasswordResetLinkByEmailHandler(UserManager<MyIdentityUser> userManager,
    IEmailSenderService emailSenderService) : IRequestHandler<SendPasswordResetLinkByEmailRequest, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkByEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.NotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var link = "https://example.com/reset-password/" + token; //todo

        var emailSendingResult = await emailSenderService
            .SendPasswordResetMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }
}