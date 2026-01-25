using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.EmailSender;
using JobSearchSiteBackend.Shared.MyAppSettings;
using JobSearchSiteBackend.Shared.MyAppSettings.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(
    IOptions<MyAppSettings> settings,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    ISendAccountCreatedEmailRunner sendAccountCreatedEmailRunner,
    StandardEmailRenderer emailRenderer,
    IOptions<MyDefaultEmailSenderSettings> emailSenderSettings) 
    : IRequestHandler<CreateAccountCommand, Result<CreateAccountResult>>
{
    public async Task<Result<CreateAccountResult>> Handle(CreateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(command.Email);

        if (userFromDb is not null)
            return Result.Conflict();

        var user = new MyIdentityUser { UserName = command.Email, Email = command.Email };

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        
        var aspNetIdentityResult = await userManager.CreateAsync(user, command.Password);

        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var domainName = settings.Value.DomainName;

        var link = $"https://{domainName}/account/confirm-email/{token}";
        
        var emailTemplate = new EmailConfirmationEmail(link);

        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);

        var senderName = emailSenderSettings.Value.Name;
        var senderEmail = emailSenderSettings.Value.EmailAddress;
        
        var emailToSend = new EmailToSend(Guid.NewGuid(), senderName, senderEmail, command.Email, 
            null, null, renderedEmail.Subject, renderedEmail.Content, renderedEmail.IsHtml);
        
        await transaction.CommitAsync(cancellationToken);

        await sendAccountCreatedEmailRunner.RunAsync(emailToSend);

        var result = new CreateAccountResult(user.Id);
        
        return Result.Success(result);
    }
}