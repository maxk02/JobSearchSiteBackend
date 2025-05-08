using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public record CreateAccountRequest(string Email, string Password) : IRequest<Result>;