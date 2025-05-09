using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetUserSessions;

public record GetUserSessionsResponse(ICollection<UserSessionDto> UserSessions);