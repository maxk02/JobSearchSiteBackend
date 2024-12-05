using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.Users.UpdateUser;

public class UpdateUserHandler(IUserRepository userRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null || currentAccountId != request.AccountId)
            return Result.Unauthorized();
        
        var existingUserObject = await userRepository.GetByAccountIdAsync(currentAccountId, cancellationToken);
        
        if (existingUserObject is null)
            return Result.NotFound();
        
        var updateUserResult = User.Create(
            request.AccountId,
            request.FirstName ?? existingUserObject.FirstName,
            request.MiddleName ?? existingUserObject.MiddleName,
            request.LastName ?? existingUserObject.LastName,
            request.DateOfBirth ?? existingUserObject.DateOfBirth,
            request.Email ?? existingUserObject.Email,
            request.Phone ?? existingUserObject.Phone,
            request.Bio ?? existingUserObject.Bio
            );

        if (updateUserResult.Value is null) return Result.WithMetadataFromResult(updateUserResult);
            
        await userRepository.UpdateAsync(updateUserResult.Value, cancellationToken);
        
        return Result.Success();
    }
}