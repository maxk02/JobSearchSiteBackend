using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetUserSessions;

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