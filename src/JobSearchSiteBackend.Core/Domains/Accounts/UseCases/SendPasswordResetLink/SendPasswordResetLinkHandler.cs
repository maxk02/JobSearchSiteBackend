using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings;
using Ardalis.Result;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public class SendPasswordResetLinkHandler(
    UserManager<MyIdentityUser> userManager,
    IOptions<MyAppSettings> injectedAppSettings,
    IEmailSenderService emailSenderService,
    IBackgroundJobService backgroundJobService,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings) : IRequestHandler<SendPasswordResetLinkCommand, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Result.Forbidden();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var domainName = injectedAppSettings.Value.DomainName;
        
        var link = $"https://{domainName}/account/reset-password/{token}";

        var emailTemplate = new ResetPasswordEmail(link);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), emailSenderSettings.Value, command.Email, null, null,
            renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);

        backgroundJobService.Enqueue(() => emailSenderService.SendEmailAsync(emailToSend, cancellationToken),
            BackgroundJobQueues.EmailSending);

        return Result.Success();
    }
}