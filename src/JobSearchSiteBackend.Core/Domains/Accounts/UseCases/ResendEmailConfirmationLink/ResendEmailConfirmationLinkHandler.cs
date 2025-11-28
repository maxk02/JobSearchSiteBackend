using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResendEmailConfirmationLink;

public class ResendEmailConfirmationLinkHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    IBackgroundJobService backgroundJobService,
    IOptions<MyAppSettings> injectedAppSettings,
    IEmailSenderService emailSenderService,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings,
    MainDataContext context) : IRequestHandler<ResendEmailConfirmationLinkCommand, Result>
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

        var emailToSend = new EmailToSend(Guid.NewGuid(), emailSenderSettings.Value, email, null, null,
            renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);
        
        backgroundJobService.Enqueue(() => emailSenderService.SendEmailAsync(emailToSend, cancellationToken),
            BackgroundJobQueues.EmailSending);

        return Result.Success();
    }
}