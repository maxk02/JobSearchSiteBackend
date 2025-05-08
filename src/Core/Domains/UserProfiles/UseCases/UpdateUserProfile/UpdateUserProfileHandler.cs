using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public class UpdateUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateUserProfileRequest, Result>
{
    public async Task<Result> Handle(UpdateUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var user = await context.UserProfiles.FindAsync([currentAccountId], cancellationToken);
        
        if (user is null)
            return Result.NotFound();
        
        if (request.FirstName is not null) user.FirstName = request.FirstName;
        if (request.LastName is not null) user.LastName = request.LastName;
        if (request.Phone is not null) user.Phone = request.Phone;
            
        context.UserProfiles.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}