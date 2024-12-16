using Core.Domains.Accounts.ChangePassword;
using Core.Domains.Accounts.ConfirmEmail;
using Core.Domains.Accounts.CreateAccount;
using Core.Domains.Accounts.DeleteAccount;
using Core.Domains.Accounts.ResetPassword;
using Core.Domains.Accounts.SendEmailConfirmationLink;
using Core.Domains.Accounts.SendPasswordResetLinkByEmail;
using Core.Domains.Accounts.SignInWithEmail;
using Shared.Result;

namespace Core.Domains.Accounts;

public interface IAccountService
{
    public Task<Result> ChangePasswordAsync(ChangePasswordRequest request,
        CancellationToken cancellationToken = default);
    public Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    public Task<Result<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);
    public Task<Result> DeleteAccountAsync(DeleteAccountRequest request, CancellationToken cancellationToken = default);
    public Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
    public Task<Result> SendEmailConfirmationLinkAsync(SendEmailConfirmationLinkRequest request,
        CancellationToken cancellationToken = default);
    public Task<Result> SendPasswordResetLinkByEmailAsync(SendPasswordResetLinkByEmailRequest request,
        CancellationToken cancellationToken = default);
    public Task<Result<SignInWithEmailResponse>> SignInWithEmailAsync(SignInWithEmailRequest request,
        CancellationToken cancellationToken = default);
}