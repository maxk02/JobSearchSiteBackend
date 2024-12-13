using API.Domains._Shared.Enums;
using Shared.Result;

namespace API.Services.Auth.AccountStorage;

public interface IAccountStorageService
{
    public Task<Result<AccountData>> SignInWithEmailAsync(string email, string password,
        CancellationToken cancellationToken = default);
    public Task<Result<AccountData>> RegisterAsync(string email, string password,
        CancellationToken cancellationToken = default);
    public Task<Result> DeleteAsync(long userId,
        CancellationToken cancellationToken = default);
    public Task<Result> AddToRoleAsync(long userId, RoleValues roleToAdd,
        CancellationToken cancellationToken = default);
    public Task<Result> RemoveFromRoleAsync(long userId, RoleValues roleToRemove,
        CancellationToken cancellationToken = default);
    public Task<Result> RemoveFromAllRolesAsync(long userId,
        CancellationToken cancellationToken = default);
    public Task<Result> ChangePasswordAsync(long userId, string oldPassword, string newPassword,
        CancellationToken cancellationToken = default);
    public Task<Result<string>> GenerateEmailConfirmationTokenByEmailAsync(string email,
        CancellationToken cancellationToken = default);
    public Task<Result<string>> GeneratePasswordResetTokenByEmailAsync(string email,
        CancellationToken cancellationToken = default);
    public Task<Result> ConfirmEmailAsync(string emailConfirmationToken,
        CancellationToken cancellationToken = default);
    public Task<Result> ResetPasswordAsync(string passwordResetToken, string newPassword,
        CancellationToken cancellationToken = default);
}