using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public class SendPasswordResetLinkHandler(
    UserManager<MyIdentityUser> userManager,
    IOptions<MyAppSettings> injectedAppSettings,
    IEmailSenderService emailSenderService,
    IBackgroundJobService backgroundJobService,
    StandardEmailRenderer emailRenderer) : IRequestHandler<SendPasswordResetLinkRequest, Result>
{
    public async Task<Result> Handle(SendPasswordResetLinkRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.NotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var domainName = injectedAppSettings.Value.DomainName;
        
        var link = $"https://{domainName}/account/reset-password/{token}";

        var emailTemplate = new ResetPasswordEmail(link);
        
        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(request.Email, renderedEmail.Subject, renderedEmail.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);

        return Result.Success();
    }
}