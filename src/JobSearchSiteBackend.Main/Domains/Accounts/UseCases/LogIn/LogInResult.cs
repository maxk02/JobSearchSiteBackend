using JobSearchSiteBackend.Main.Domains.Accounts.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

public record LogInResult(string TokenId, AccountDataDto AccountData);