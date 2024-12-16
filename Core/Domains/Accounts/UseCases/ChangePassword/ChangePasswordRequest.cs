namespace Core.Domains.Accounts.UseCases.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string NewPassword);