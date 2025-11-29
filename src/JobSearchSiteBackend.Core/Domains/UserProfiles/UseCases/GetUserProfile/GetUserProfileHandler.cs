using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;

public class GetUserProfileHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService) : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResult>>
{
    public async Task<Result<GetUserProfileResult>> Handle(GetUserProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        var userWithEmailAndAvatars = await context.UserProfiles
            .Where(u => u.Id == currentAccountId)
            .Select(u => new
            {
                User = u,
                Email = u.Account!.Email,
                Avatars = u.UserAvatars
            })
            .SingleOrDefaultAsync(cancellationToken);
        
        var user = userWithEmailAndAvatars?.User;
        var email = userWithEmailAndAvatars?.Email;

        if (user is null || email is null)
            return Result.Error();

        var avatar = userWithEmailAndAvatars!.Avatars?.GetLatestAvailableAvatar();

        string? avatarLink = null;

        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars, 
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }

        var result = new GetUserProfileResult(user.FirstName, user.LastName, email, user.Phone, 
            avatarLink, user.IsReceivingApplicationStatusUpdates);
        
        return Result.Success(result);
    }
}