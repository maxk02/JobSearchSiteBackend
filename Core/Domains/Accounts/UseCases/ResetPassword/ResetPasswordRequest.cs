namespace Core.Domains.Accounts.UseCases.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword);