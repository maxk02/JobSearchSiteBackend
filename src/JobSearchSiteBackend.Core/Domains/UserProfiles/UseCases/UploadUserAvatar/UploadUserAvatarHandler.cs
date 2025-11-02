using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;

public class UploadUserAvatarHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<UploadUserAvatarRequest, Result<UploadUserAvatarResponse>>
{
    public async Task<Result<UploadUserAvatarResponse>> Handle(UploadUserAvatarRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId != request.UserId) 
            return Result.Forbidden();
        
        var existingAvatars = context.UserAvatars.Where(a => a.UserId == request.UserId).ToList();
        foreach (var existingAvatar in existingAvatars)
        {
            existingAvatar.IsDeleted = true;
        }
        
        var userAvatar = new UserAvatar(request.UserId, request.Extension, request.Size);
        
        context.UserAvatars.Add(userAvatar);
        context.UserAvatars.UpdateRange(existingAvatars);
        await context.SaveChangesAsync(cancellationToken);
        
        return new UploadUserAvatarResponse(userAvatar.Id);
    }
}