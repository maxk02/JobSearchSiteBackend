using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Account;

public record LogInResponseDto(string TokenId, AccountDataDto AccountData);