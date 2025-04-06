using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.EmailMessages;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.MyAppSettings;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public class SendEmailConfirmationLinkHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    IBackgroundJobService backgroundJobService,
    IOptions<MyAppSettings> injectedAppSettings,
    IEmailSenderService emailSenderService) : IRequestHandler<SendEmailConfirmationLinkRequest, Result>
{
    public async Task<Result> Handle(SendEmailConfirmationLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        
        if (user is null)
            return Result.NotFound();
        
        var email = user.Email;
        
        if (email is null)
            return Result.Forbidden();
        
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        
        if (isEmailConfirmed)
            return Result.Forbidden();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var domainName = injectedAppSettings.Value.DomainName;
        
        var link = $"https://{domainName}/account/confirm-email/{token}";

        var emailToSend = new EmailConfirmationEmail(link);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(email, emailToSend.Subject, emailToSend.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);

        return Result.Success();
    }
}