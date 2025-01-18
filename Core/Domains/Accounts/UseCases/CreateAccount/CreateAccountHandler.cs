using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.EmailMessages;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.MyAppSettings;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(
    IOptions<MyAppSettings> settings,
    UserManager<MyIdentityUser> userManager,
    IBackgroundJobService backgroundJobService,
    IEmailSenderService emailSenderService) 
    : IRequestHandler<CreateAccountRequest, Result<CreateAccountResponse>>
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(request.Email);

        if (userFromDb is not null)
            return Result<CreateAccountResponse>.Conflict();

        var user = new MyIdentityUser { Email = request.Email };

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var aspNetIdentityResult = await userManager.CreateAsync(user, request.Password);

        if (!aspNetIdentityResult.Succeeded)
            return Result<CreateAccountResponse>.Error();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var domainName = settings.Value.DomainName;

        var link = $"https://{domainName}/account/confirm-email/{token}";
        
        var emailToSend = new EmailConfirmationEmail(link);

        backgroundJobService.Enqueue(() => emailSenderService
                .SendEmailAsync(request.Email, emailToSend.Subject, emailToSend.Content, CancellationToken.None),
            BackgroundJobQueues.EmailSending);
        
        transaction.Complete();

        return new CreateAccountResponse(token);
    }
}