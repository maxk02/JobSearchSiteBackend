using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public class AddUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddUserProfileRequest, Result<AddUserProfileResponse>>
{
    public async Task<Result<AddUserProfileResponse>> Handle(AddUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.AccountId)
            return Result<AddUserProfileResponse>.Forbidden();
        
        var newUser = new UserProfile(request.AccountId, request.FirstName, request.MiddleName,
            request.LastName, request.DateOfBirth, request.Email, request.Phone);
        
        await context.UserProfiles.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddUserProfileResponse(newUser.Id);
    }
}