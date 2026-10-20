using JobSearchSiteBackend.Main.Domains.Accounts.UseCases.ChangePassword;

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