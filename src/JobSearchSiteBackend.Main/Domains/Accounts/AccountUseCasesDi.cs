using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.DeleteAccount;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetAccountData;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogOut;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResendEmailConfirmationLink;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetForgottenPassword;
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
        serviceCollection.AddScoped<GetAccountDataHandler>();
        serviceCollection.AddScoped<LogInHandler>();
        serviceCollection.AddScoped<LogOutHandler>();
        serviceCollection.AddScoped<ResendEmailConfirmationLinkHandler>();
        serviceCollection.AddScoped<ResetForgottenPasswordHandler>();
        serviceCollection.AddScoped<SendPasswordResetLinkHandler>();
    }
}