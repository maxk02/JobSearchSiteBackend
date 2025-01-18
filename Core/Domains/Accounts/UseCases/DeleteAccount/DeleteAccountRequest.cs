using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public record DeleteAccountRequest : IRequest<Result>;