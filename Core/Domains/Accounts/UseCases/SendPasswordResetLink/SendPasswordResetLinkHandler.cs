using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.EmailMessages;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.MyAppSettings;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public class SendPasswordResetLinkHandler(
    UserManager<MyIdentityUser> userManager,
    IOptions<MyAppSettings> injectedAppSettings,
    IEmailSenderService emailSenderService,
    IBackgroundJobService backgroundJobService) : IRequestHandler<SendPasswordResetLinkRequest, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.NotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var domainName = injectedAppSettings.Value.DomainName;
        
        var link = $"https://{domainName}/account/reset-password/{token}";

        var emailToSend = new ResetPasswordEmail(link);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(request.Email, emailToSend.Subject, emailToSend.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);

        return Result.Success();
    }
}