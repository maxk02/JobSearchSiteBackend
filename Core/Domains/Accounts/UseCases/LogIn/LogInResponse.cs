using Core.Domains.Accounts.Dtos;

namespace Core.Domains.Accounts.UseCases.LogIn;

public record LogInResponse(AccountDataDto AccountData, string Token, string TokenId);