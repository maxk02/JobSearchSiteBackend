using Domain.Shared.Enums;
using Shared.Result;

namespace Application.Services.Auth;

public interface IAccountService
{
    Task<Result<UserClaimsDto>> SignInWithEmail(string email, string password);
    Task<Result<UserClaimsDto>> Register(string email, string password);
    Task<Result> Delete(string id);
    Task<Result> ChangeRole(string id, RoleValues newRoleValue);
    Task<Result> ChangePassword(string id, string oldPassword, string newPassword);
    Task<Result<string>> GenerateEmailConfirmationToken(string id);
    Task<Result<string>> GeneratePasswordResetToken(string userId);
    Task<Result> ConfirmEmail(string id, string emailConfirmationToken);
    Task<Result> ResetPassword(string id, string passwordResetToken, string newPassword);
}