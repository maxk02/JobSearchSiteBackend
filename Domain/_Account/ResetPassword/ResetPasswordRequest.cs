namespace Domain._Account.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword);