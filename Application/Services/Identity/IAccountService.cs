using Domain.Enums;

namespace Application.Services.Identity;

public interface IAccountService
{
    Task<string?> SignInWithEmail(string email, string password);
    Task<string?> Register(string email, string password);
    Task<bool> Delete(string id);
    Task<bool> ChangeRole(string id, RoleValue newRoleValue);
    Task<bool> ChangePassword(string id, string oldPassword, string newPassword);
    Task<string?> GenerateEmailConfirmationToken(string id);
    Task<string?> GeneratePasswordResetToken(string id);
    Task<bool> ConfirmEmail(string id, string emailConfirmationToken);
    Task<bool> ResetPassword(string id, string passwordResetToken, string newPassword);
}