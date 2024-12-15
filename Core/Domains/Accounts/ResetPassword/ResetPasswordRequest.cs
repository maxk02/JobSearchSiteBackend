namespace Core.Domains.Accounts.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword);