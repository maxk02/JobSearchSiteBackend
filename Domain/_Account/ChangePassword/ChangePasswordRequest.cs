namespace Domain._Account.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string NewPassword);