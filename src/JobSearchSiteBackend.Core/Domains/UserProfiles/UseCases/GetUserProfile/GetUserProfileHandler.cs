using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;

public class GetUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResult>>
{
    public async Task<Result<GetUserProfileResult>> Handle(GetUserProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        // var user = await context.UserProfiles.FindAsync([currentAccountId], cancellationToken);
        
        var userWithEmail = await context.UserProfiles
            .Where(u => u.Id == currentAccountId)
            .Select(u => new { User = u, Email = u.Account!.Email })
            .SingleOrDefaultAsync(cancellationToken);

        var user = userWithEmail?.User;
        var email = userWithEmail?.Email;

        if (user is null || email is null)
            return Result<GetUserProfileResult>.Error();

        return new GetUserProfileResult(user.FirstName, user.LastName, email, user.Phone, ""); // todo avatar
    }
}