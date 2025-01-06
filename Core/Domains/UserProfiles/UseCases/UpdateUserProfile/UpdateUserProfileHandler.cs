using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public class UpdateUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateUserProfileRequest, Result>
{
    public async Task<Result> Handle(UpdateUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.Id) return Result.Forbidden();
        
        var existingUserObject = await context.UserProfiles
            .Include(u => u.Phone)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        
        if (existingUserObject is null)
            return Result.Error();
        
        var updateUserResult = UserProfile.Create(
            request.Id,
            request.FirstName ?? existingUserObject.FirstName,
            request.MiddleName ?? existingUserObject.MiddleName,
            request.LastName ?? existingUserObject.LastName,
            request.DateOfBirth ?? existingUserObject.DateOfBirth,
            request.Email ?? existingUserObject.Email,
            request.Phone ?? existingUserObject.Phone
            );

        if (updateUserResult.IsFailure)
            return Result.WithMetadataFrom(updateUserResult);
        
        var updatedUser = updateUserResult.Value;
            
        context.UserProfiles.Update(updatedUser);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}