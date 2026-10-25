using JobSearchSiteBackend.Main.Services.EmailSender;
using JobSearchSiteBackend.Main.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Main.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Main.MyAppSettings.Email;
using JobSearchSiteBackend.Main.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.CreateAccount;

public static class CreateAccount
{
    public record Request(string Email, string Password);
    public record Response(long Id);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("", Handle)
            .AllowAnonymous();

    public static async Task<Results<Ok<Response>, Conflict, ProblemHttpResult>> Handle(
        [FromBody] Request request,
        IOptions<MyAppSettings.MyAppSettings> settings,
        UserManager<MyIdentityUser> userManager,
        MainDataContext context,
        ISendAccountCreatedEmailRunner sendAccountCreatedEmailRunner,
        StandardEmailRenderer emailRenderer,
        IOptions<MyDefaultEmailSenderSettings> emailSenderSettings,
        CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return TypedResults.Conflict();

        var user = new MyIdentityUser { UserName = request.Email, Email = request.Email };

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var identityResult = await userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
            return TypedResults.Problem("Could not create account.", statusCode: StatusCodes.Status400BadRequest);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var domainName = settings.Value.FrontendDomainName;
        var link = $"https://{domainName}/account/confirm-email?token={token}";

        var renderedEmail = await emailRenderer.RenderAsync(new EmailConfirmationEmail(link));

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

        await transaction.CommitAsync(cancellationToken);

        await sendAccountCreatedEmailRunner.RunAsync(emailToSend);

        return TypedResults.Ok(new Response(user.Id));
    }
}