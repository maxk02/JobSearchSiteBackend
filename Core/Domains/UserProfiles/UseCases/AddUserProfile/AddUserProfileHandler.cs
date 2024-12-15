using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public class AddUserProfileHandler(IUserProfileRepository userProfileRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result<AddUserProfileResponse>> Handle(AddUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result<AddUserProfileResponse>.Unauthorized();
        
        if (currentAccountId != request.AccountId)
            return Result<AddUserProfileResponse>.Forbidden();
        
        var createUserResult = UserProfile.Create(request.AccountId, request.FirstName, request.MiddleName,
            request.LastName, request.DateOfBirth, request.Email, request.Phone, request.Bio);
        if (createUserResult.Value is null)
            return Result<AddUserProfileResponse>.WithMetadataFrom(createUserResult);
        
        var addedToDbUser = await userProfileRepository.AddAsync(createUserResult.Value, cancellationToken);
        
        return new AddUserProfileResponse(addedToDbUser.Id);
    }
}