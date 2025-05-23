﻿using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JobSearchSiteBackend.Shared.MyAppSettings;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;

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