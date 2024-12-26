using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword) : IRequest<Result>;