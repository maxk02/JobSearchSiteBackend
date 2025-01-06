using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
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
        
        var createUserResult = UserProfile.Create(request.AccountId, request.FirstName, request.MiddleName,
            request.LastName, request.DateOfBirth, request.Email, request.Phone);
        if (createUserResult.IsFailure)
            return Result<AddUserProfileResponse>.WithMetadataFrom(createUserResult);
        
        var newUser = createUserResult.Value;
        
        await context.UserProfiles.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddUserProfileResponse(newUser.Id);
    }
}