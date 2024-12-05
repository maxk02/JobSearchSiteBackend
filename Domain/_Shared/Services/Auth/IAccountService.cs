using Domain._Shared.Enums;
using Shared.Result;

namespace Domain._Shared.Services.Auth;

public interface IAccountService
{
    public Task<Result<UserClaimsDto>> SignInWithEmailAsync(string email, string password,
        CancellationToken cancellationToken = default);
    public Task<Result<UserClaimsDto>> RegisterAsync(string email, string password,
        CancellationToken cancellationToken = default);
    public Task<Result> DeleteAsync(string id,
        CancellationToken cancellationToken = default);
    public Task<Result> AddToRoleAsync(string userId, RoleValues roleToAdd,
        CancellationToken cancellationToken = default);
    public Task<Result> RemoveFromRoleAsync(string userId, RoleValues roleToRemove,
        CancellationToken cancellationToken = default);
    public Task<Result> RemoveFromAllRolesAsync(string userId,
        CancellationToken cancellationToken = default);
    public Task<Result> ChangePasswordAsync(string id, string oldPassword, string newPassword,
        CancellationToken cancellationToken = default);
    public Task<Result<string>> GenerateEmailConfirmationTokenAsync(string id,
        CancellationToken cancellationToken = default);
    public Task<Result<string>> GeneratePasswordResetTokenAsync(string id,
        CancellationToken cancellationToken = default);
    public Task<Result> ConfirmEmailAsync(string id, string emailConfirmationToken,
        CancellationToken cancellationToken = default);
    public Task<Result> ResetPasswordAsync(string id, string passwordResetToken, string newPassword,
        CancellationToken cancellationToken = default);
}