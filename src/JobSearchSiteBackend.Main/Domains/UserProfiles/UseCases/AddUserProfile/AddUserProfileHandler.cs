using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;

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