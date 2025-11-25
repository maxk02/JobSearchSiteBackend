using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(
    IOptions<MyAppSettings> settings,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IBackgroundJobService backgroundJobService,
    IEmailSenderService emailSenderService,
    StandardEmailRenderer emailRenderer) 
    : IRequestHandler<CreateAccountCommand, Result<CreateAccountResult>>
{
    public async Task<Result<CreateAccountResult>> Handle(CreateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(command.Email);

        if (userFromDb is not null)
            return Result.Conflict();

        var user = new MyIdentityUser { Email = command.Email };

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        
        var aspNetIdentityResult = await userManager.CreateAsync(user, command.Password);

        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var domainName = settings.Value.DomainName;

        var link = $"https://{domainName}/account/confirm-email/{token}";
        
        var emailTemplate = new EmailConfirmationEmail(link);

        var renderedEmail = await emailRenderer.RenderAsync(emailTemplate);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(command.Email, renderedEmail.Subject, renderedEmail.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);
        
        await transaction.CommitAsync(cancellationToken);

        var result = new CreateAccountResult(user.Id);
        
        return Result.Success(result);
    }
}