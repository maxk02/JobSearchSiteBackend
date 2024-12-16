namespace Core.Domains.Accounts.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string NewPassword);