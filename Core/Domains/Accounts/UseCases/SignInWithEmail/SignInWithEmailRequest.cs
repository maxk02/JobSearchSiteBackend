namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public record SignInWithEmailRequest(string Email, string Password);