using Core.Domains.Accounts.Dtos;

namespace API.Controllers.Account;

public record LogInResponseDto(string TokenId, AccountDataDto AccountData);