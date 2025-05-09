using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;

public class AddUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddUserProfileRequest, Result<AddUserProfileResponse>>
{
    public async Task<Result<AddUserProfileResponse>> Handle(AddUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        var newUser = new UserProfile(currentAccountId, request.FirstName,
            request.LastName, request.Email, request.Phone);
        
        await context.UserProfiles.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddUserProfileResponse(newUser.Id);
    }
}