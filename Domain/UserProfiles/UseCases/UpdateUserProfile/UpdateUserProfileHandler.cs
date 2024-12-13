using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.UserProfiles.UseCases.UpdateUserProfile;

public class UpdateUserProfileHandler(IUserProfileRepository userProfileRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(UpdateUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null) return Result.Unauthorized();
        
        if (currentAccountId != request.Id) return Result.Forbidden();
        
        
        var existingUserObject = await userProfileRepository.GetByIdAsync(currentAccountId.Value, cancellationToken);
        
        if (existingUserObject is null)
            return Result.Error();
        
        var updateUserResult = UserProfile.Create(
            request.Id,
            request.FirstName ?? existingUserObject.FirstName,
            request.MiddleName ?? existingUserObject.MiddleName,
            request.LastName ?? existingUserObject.LastName,
            request.DateOfBirth ?? existingUserObject.DateOfBirth,
            request.Email ?? existingUserObject.Email,
            request.Phone ?? existingUserObject.Phone,
            request.Bio ?? existingUserObject.Bio
            );

        if (updateUserResult.Value is null) return Result.WithMetadataFrom(updateUserResult);
            
        await userProfileRepository.UpdateAsync(updateUserResult.Value, cancellationToken);
        
        return Result.Success();
    }
}