using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public record SendPasswordResetLinkRequest(string Email) : IRequest<Result>;