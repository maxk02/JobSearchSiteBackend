using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLinkByEmail;

public record SendPasswordResetLinkByEmailRequest(string Email) : IRequest<Result>;