using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public class UpdateUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateUserProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateUserProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var user = await context.UserProfiles.FindAsync([currentAccountId], cancellationToken);
        
        if (user is null)
            return Result.NotFound();
        
        if (command.FirstName is not null) user.FirstName = command.FirstName;
        if (command.LastName is not null) user.LastName = command.LastName;
        if (command.Phone is not null) user.Phone = command.Phone;
            
        context.UserProfiles.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}