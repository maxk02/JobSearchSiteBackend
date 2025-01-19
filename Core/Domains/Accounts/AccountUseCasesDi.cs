using Core.Domains.Accounts.UseCases.ChangePassword;
using Core.Domains.Accounts.UseCases.ConfirmEmail;
using Core.Domains.Accounts.UseCases.CreateAccount;
using Core.Domains.Accounts.UseCases.DeleteAccount;
using Core.Domains.Accounts.UseCases.ExtendSession;
using Core.Domains.Accounts.UseCases.GetUserSessions;
using Core.Domains.Accounts.UseCases.LogIn;
using Core.Domains.Accounts.UseCases.LogOut;
using Core.Domains.Accounts.UseCases.ResetPassword;
using Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;
using Core.Domains.Accounts.UseCases.SendPasswordResetLink;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.Accounts;

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