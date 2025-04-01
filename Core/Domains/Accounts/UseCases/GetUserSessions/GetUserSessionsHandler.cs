using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Accounts.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Accounts.UseCases.GetUserSessions;

public class GetUserSessionsHandler(ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetUserSessionsRequest, Result<GetUserSessionsResponse>>
{
    public async Task<Result<GetUserSessionsResponse>> Handle(GetUserSessionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var query = context.UserSessions
            .Where(us => us.UserId == currentAccountId);

        var userSessionDtos = await query
            .ProjectTo<UserSessionDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var response = new GetUserSessionsResponse(userSessionDtos);

        return response;
    }
}