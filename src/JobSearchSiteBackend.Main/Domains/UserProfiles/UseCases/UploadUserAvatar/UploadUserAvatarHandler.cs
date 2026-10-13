using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;

public class UploadUserAvatarHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<UploadUserAvatarCommand, Result<UploadUserAvatarResult>>
{
    public async Task<Result<UploadUserAvatarResult>> Handle(UploadUserAvatarCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId != command.UserId) 
            return Result.Forbidden();
        
        var existingAvatars = context.UserAvatars.Where(a => a.UserId == command.UserId).ToList();
        foreach (var existingAvatar in existingAvatars)
        {
            existingAvatar.IsDeleted = true;
        }
        
        var userAvatar = new UserAvatar(command.UserId, command.Extension, command.Size);
        
        context.UserAvatars.Add(userAvatar);
        context.UserAvatars.UpdateRange(existingAvatars);
        await context.SaveChangesAsync(cancellationToken);
        
        return new UploadUserAvatarResult(userAvatar.Id);
    }
}