using Core.Domains.Accounts.Dtos;

namespace Core.Domains.Accounts.UseCases.GetUserSessions;

public record GetUserSessionsResponse(ICollection<UserSessionDto> UserSessions);