using JobSearchSiteBackend.Main.Attributes;
using JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Main.MyAppSettings.Email;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Services.EmailSender;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ResendEmailConfirmationLink;

public static class ResendEmailConfirmationLink
{
    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("send-email-confirmation-link", Handle)
            .WithMetadata(new AllowUnconfirmedEmailAttribute());

    public static async Task<Results<Ok, NotFound, ForbidHttpResult>> Handle(
        ICurrentAccountService currentAccountService,
        UserManager<MyIdentityUser> userManager,
        IResendEmailConfirmationLinkRunner resendEmailConfirmationLinkRunner,
        IOptions<MyAppSettings.MyAppSettings> injectedAppSettings,
        StandardEmailRenderer emailRenderer,
        IOptions<MyDefaultEmailSenderSettings> emailSenderSettings,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user is null)
            return TypedResults.NotFound();

        if (user.Email is null || await userManager.IsEmailConfirmedAsync(user))
            return TypedResults.Forbid();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var domainName = injectedAppSettings.Value.FrontendDomainName;
        var link = $"https://{domainName}/account/confirm-email?token={token}";

        var renderedEmail = await emailRenderer.RenderAsync(new EmailConfirmationEmail(link));

        var emailToSend = new EmailToSend(
            Guid.NewGuid(),
            emailSenderSettings.Value.Name,
            emailSenderSettings.Value.EmailAddress,
            user.Email,
            null,
            null,
            renderedEmail.Subject,
            renderedEmail.Content,
            renderedEmail.IsHtml);

        await resendEmailConfirmationLinkRunner.RunAsync(emailToSend);

        return TypedResults.Ok();
    }
}