using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public class SendPasswordResetLinkHandler(
    UserManager<MyIdentityUser> userManager,
    IOptions<MyAppSettings> injectedAppSettings,
    ISendPasswordResetLinkRunner sendPasswordResetLinkRunner,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings) : IRequestHandler<SendPasswordResetLinkCommand, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Result.Forbidden();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var domainName = injectedAppSettings.Value.FrontendDomainName;
        
        var link = $"https://{domainName}/account/reset-password?token={token}";

        var emailTemplate = new ResetPasswordEmail(link);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);
        
        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, command.Email,
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);

        await sendPasswordResetLinkRunner.RunAsync(emailToSend);

        return Result.Success();
    }
}