namespace Core._Account.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string NewPassword);