using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.EmailMessages;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.MyAppSettings;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(
    IOptions<MyAppSettings> settings,
    UserManager<MyIdentityUser> userManager,
    IBackgroundJobService backgroundJobService,
    IEmailSenderService emailSenderService) 
    : IRequestHandler<CreateAccountRequest, Result>
{
    public async Task<Result> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(request.Email);

        if (userFromDb is not null)
            return Result.Conflict();

        var user = new MyIdentityUser { Email = request.Email };

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var aspNetIdentityResult = await userManager.CreateAsync(user, request.Password);

        if (!aspNetIdentityResult.Succeeded)
            return Result.Error();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var domainName = settings.Value.DomainName;

        var link = $"https://{domainName}/account/confirm-email/{token}";
        
        var emailToSend = new EmailConfirmationEmail(link);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(request.Email, emailToSend.Subject, emailToSend.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);
        
        transaction.Complete();

        return Result.Success();
    }
}