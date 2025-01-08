using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
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
        
        var query = context.UserProfiles
            .Include(u => u.Phone)
            .Where(u => u.Id == request.Id);

        var userProfile = await query.SingleOrDefaultAsync(cancellationToken);
        
        if (userProfile is null)
            return Result.Error();
        
        if (request.FirstName is not null) userProfile.FirstName = request.FirstName;
        if (request.MiddleName is not null) userProfile.MiddleName = request.MiddleName;
        if (request.LastName is not null) userProfile.LastName = request.LastName;
        if (request.DateOfBirth is not null) userProfile.DateOfBirth = request.DateOfBirth;
        if (request.Email is not null) userProfile.Email = request.Email;
        if (request.Phone is not null) userProfile.Phone = request.Phone;
            
        context.UserProfiles.Update(userProfile);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}