using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Account.Dtos;

public record LogInResponse(string TokenId, AccountDataDto AccountData);