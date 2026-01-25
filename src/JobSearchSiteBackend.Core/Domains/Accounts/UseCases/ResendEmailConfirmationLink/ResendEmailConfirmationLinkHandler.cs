using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResendEmailConfirmationLink;

public class ResendEmailConfirmationLinkHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    IResendEmailConfirmationLinkRunner resendEmailConfirmationLinkRunner,
    IOptions<MyAppSettings> injectedAppSettings,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings) : IRequestHandler<ResendEmailConfirmationLinkCommand, Result>
{
    public async Task<Result> Handle(ResendEmailConfirmationLinkCommand command,
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

        var emailTemplate = new EmailConfirmationEmail(link);

        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);

        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, email,
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);

        await resendEmailConfirmationLinkRunner.RunAsync(emailToSend);

        return Result.Success();
    }
}