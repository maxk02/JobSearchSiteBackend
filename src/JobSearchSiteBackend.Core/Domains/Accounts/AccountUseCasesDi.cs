﻿using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.DeleteAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ExtendSession;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetUserSessions;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogOut;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetPassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.Accounts;

public static class AccountUseCasesDi
{
    public static void ConfigureAccountUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ChangePasswordHandler>();
        serviceCollection.AddScoped<ConfirmEmailHandler>();
        serviceCollection.AddScoped<CreateAccountHandler>();
        serviceCollection.AddScoped<DeleteAccountHandler>();
        serviceCollection.AddScoped<ExtendSessionHandler>();
        serviceCollection.AddScoped<GetUserSessionsHandler>();
        serviceCollection.AddScoped<LogInHandler>();
        serviceCollection.AddScoped<LogOutHandler>();
        serviceCollection.AddScoped<ResetPasswordHandler>();
        serviceCollection.AddScoped<SendEmailConfirmationLinkHandler>();
        serviceCollection.AddScoped<SendPasswordResetLinkHandler>();
    }
}