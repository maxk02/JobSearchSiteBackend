using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Accounts.UseCases.GetUserSessions;

public class GetUserSessionsHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetUserSessionsRequest, GetUserSessionsResponse>
{
    public async Task<GetUserSessionsResponse> Handle(GetUserSessionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var query = context.UserSessions
            .Where(us => us.UserId == currentAccountId);

        var userSessions = await query.ToListAsync(cancellationToken);

        var userSessionDtos = userSessions
            .Select(x => new UserSessionDto(x.TokenId, x.FirstTimeIssuedUtc,
                x.LastDevice, x.LastOs, x.LastClient))
            .ToList();

        var response = new GetUserSessionsResponse(userSessionDtos);

        return response;
    }
}