using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

public record LogInResponse(string TokenId, AccountDataDto AccountData);