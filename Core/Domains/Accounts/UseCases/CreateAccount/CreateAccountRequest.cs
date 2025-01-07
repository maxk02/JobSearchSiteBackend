using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public record CreateAccountRequest(string Email, string Password,
    string? Device, string? Os, string? Client) : IRequest<Result<CreateAccountResponse>>;