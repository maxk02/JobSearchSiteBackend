using Core.Domains.Accounts.UseCases.ChangePassword;
using Core.Domains.Accounts.UseCases.ConfirmEmail;
using Core.Domains.Accounts.UseCases.CreateAccount;
using Core.Domains.Accounts.UseCases.DeleteAccount;
using Core.Domains.Accounts.UseCases.ResetPassword;
using Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;
using Core.Domains.Accounts.UseCases.SendPasswordResetLinkByEmail;
using Core.Domains.Accounts.UseCases.SignInWithEmail;
using Core.Domains.UserProfiles;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Core.Services.EmailSender;
using Shared.Result;

namespace Core.Domains.Accounts;

public class AccountService(IAccountStorageService accountStorageService,
    ICurrentAccountService currentAccountService,
    IAccountTokenGenerationService accountTokenGenerationService,
    IUserProfileRepository userProfileRepository,
    IEmailSenderService emailSenderService) : IAccountService
{
    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var changePasswordResult = await accountStorageService
            .ChangePasswordAsync(currentUserId, request.OldPassword, request.NewPassword, cancellationToken);
        
        return changePasswordResult;
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var confirmEmailResult = await accountStorageService
            .ConfirmEmailAsync(request.Token, cancellationToken);
        
        return confirmEmailResult;
    }

    public async Task<Result<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var accServiceCreationResult = await accountStorageService
            .RegisterAsync(request.Email, request.Password, cancellationToken);
        
        if (accServiceCreationResult.Value is null)
            return Result<CreateAccountResponse>.WithMetadataFrom(accServiceCreationResult);

        var token = accountTokenGenerationService.Generate(accServiceCreationResult.Value);

        return new CreateAccountResponse(token);
    }

    public async Task<Result> DeleteAccountAsync(DeleteAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetIdOrThrow() != request.Id)
            return Result.Forbidden();
        
        var deletionResult = await accountStorageService.DeleteAsync(request.Id, cancellationToken);
        
        if (!deletionResult.IsSuccess)
        {
            return deletionResult;
        }
        
        var userToRemove = await userProfileRepository.GetByIdAsync(request.Id, CancellationToken.None);
        if (userToRemove is not null)
        {
            await userProfileRepository.RemoveAsync(userToRemove, CancellationToken.None);
        }

        return deletionResult;
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var passwordResetResult = await accountStorageService
            .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
        
        return passwordResetResult;
    }

    public async Task<Result> SendEmailConfirmationLinkAsync(SendEmailConfirmationLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetEmailOrThrow() != request.Email)
            return Result.Forbidden();
        
        var tokenGenerationResult = await accountStorageService
            .GenerateEmailConfirmationTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/confirm-email/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendEmailConfirmationMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }

    public async Task<Result> SendPasswordResetLinkByEmailAsync(SendPasswordResetLinkByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var tokenGenerationResult = await accountStorageService
            .GeneratePasswordResetTokenByEmailAsync(request.Email, cancellationToken);

        if (tokenGenerationResult.Value is null)
            return Result.WithMetadataFrom(tokenGenerationResult);
        
        var link = "https://example.com/reset-password/" + tokenGenerationResult.Value; //todo

        var emailSendingResult = await emailSenderService
            .SendPasswordResetMessageAsync(request.Email, link, cancellationToken);
        
        return emailSendingResult;
    }

    public async Task<Result<SignInWithEmailResponse>> SignInWithEmailAsync(SignInWithEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var matchedAccountDataResult = await accountStorageService
            .SignInWithEmailAsync(request.Email, request.Password, cancellationToken);

        if (matchedAccountDataResult.Value is null)
            return Result<SignInWithEmailResponse>.WithMetadataFrom(matchedAccountDataResult);
        
        var token = accountTokenGenerationService.Generate(matchedAccountDataResult.Value);

        return new SignInWithEmailResponse(token);
    }
}