using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.EntityFrameworkCore;
using JobSearchSiteBackend.Main.Persistence;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddUserProfile;

public class AddUserProfileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddUserProfileCommand, Result<AddUserProfileResult>>
{
    public async Task<Result<AddUserProfileResult>> Handle(AddUserProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        var newUser = new UserProfile(currentAccountId, command.FirstName,
            command.LastName, command.Phone, true);
        
        await context.UserProfiles.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddUserProfileResult(newUser.Id);
    }
}