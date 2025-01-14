using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.LogIn;

public record LogInRequest(string Email, string Password,
    string? Device, string? Os, string? Client) : IRequest<Result<LogInResponse>>;