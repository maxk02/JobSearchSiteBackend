using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public record SignInWithEmailRequest(string Email, string Password) : IRequest<Result<SignInWithEmailResponse>>;