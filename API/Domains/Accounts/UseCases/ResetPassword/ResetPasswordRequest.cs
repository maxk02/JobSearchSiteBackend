namespace API.Domains.Accounts.UseCases.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword);