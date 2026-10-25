using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ChangePassword;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ConfirmEmail;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.CreateAccount;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.DeleteAccount;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.GetAccountData;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.LogIn;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.LogOut;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ResendEmailConfirmationLink;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ResetForgottenPassword;
using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.SendPasswordResetLink;

namespace JobSearchSiteBackend.Main.Domains.Accounts;

public static class AccountsEndpoints
{
    public static IEndpointRouteBuilder MapAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/account")
            .WithTags("Accounts")
            .RequireAuthorization();

        ChangePassword.MapEndpoint(group);
        ConfirmEmail.MapEndpoint(group);
        CreateAccount.MapEndpoint(group);
        DeleteAccount.MapEndpoint(group);
        GetAccountData.MapEndpoint(group);
        LogIn.MapEndpoint(group);
        LogOut.MapEndpoint(group);
        ResendEmailConfirmationLink.MapEndpoint(group);
        ResetForgottenPassword.MapEndpoint(group);
        SendPasswordResetLink.MapEndpoint(group);

        return app;
    }
}