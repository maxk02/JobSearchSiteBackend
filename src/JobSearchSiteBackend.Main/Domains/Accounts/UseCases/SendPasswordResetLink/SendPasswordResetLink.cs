using JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Main.MyAppSettings.Email;
using JobSearchSiteBackend.Main.Services.EmailSender;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.SendPasswordResetLink;

public static class SendPasswordResetLink
{
    public record Request(string Email);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("send-password-reset-link", Handle)
            .AllowAnonymous();

    public static async Task<Results<Ok, ForbidHttpResult>> Handle(
        [FromBody] Request request,
        UserManager<MyIdentityUser> userManager,
        IOptions<MyAppSettings.MyAppSettings> injectedAppSettings,
        ISendPasswordResetLinkRunner sendPasswordResetLinkRunner,
        StandardEmailRenderer emailRenderer,
        IOptions<MyDefaultEmailSenderSettings> emailSenderSettings,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return TypedResults.Forbid();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var domainName = injectedAppSettings.Value.FrontendDomainName;
        var link = $"https://{domainName}/account/reset-password?token={token}";

        var renderedEmail = await emailRenderer.RenderAsync(new ResetPasswordEmail(link));

        var emailToSend = new EmailToSend(
            Guid.NewGuid(),
            emailSenderSettings.Value.Name,
            emailSenderSettings.Value.EmailAddress,
            request.Email,
            null,
            null,
            renderedEmail.Subject,
            renderedEmail.Content,
            renderedEmail.IsHtml);

        await sendPasswordResetLinkRunner.RunAsync(emailToSend);

        return TypedResults.Ok();
    }
}