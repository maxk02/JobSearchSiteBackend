using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ConfirmEmail;

public record ConfirmEmailRequest(string Token) : IRequest<Result>;