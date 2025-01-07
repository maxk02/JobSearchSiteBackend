using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public record SignInWithEmailRequest(string Email, string Password,
    string? Device, string? Os, string? Client) : IRequest<Result<SignInWithEmailResponse>>;