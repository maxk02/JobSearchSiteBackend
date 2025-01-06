using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public record DeleteAccountRequest(long Id, string Token) : IRequest<Result>;