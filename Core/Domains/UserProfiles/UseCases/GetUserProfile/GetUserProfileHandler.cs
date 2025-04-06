using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfile;

public class GetUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetUserProfileRequest, Result<GetUserProfileResponse>>
{
    public async Task<Result<GetUserProfileResponse>> Handle(GetUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        var user = await context.UserProfiles.FindAsync([currentAccountId], cancellationToken);
        
        if (user is null)
            return Result<GetUserProfileResponse>.NotFound();

        return new GetUserProfileResponse(user.FirstName, user.LastName, user.Email, user.Phone);
    }
}