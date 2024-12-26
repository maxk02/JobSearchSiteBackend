using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string NewPassword) : IRequest<Result>;