using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword) : IRequest<Result>;