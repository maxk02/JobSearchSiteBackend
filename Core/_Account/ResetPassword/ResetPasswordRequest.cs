namespace Core._Account.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword);