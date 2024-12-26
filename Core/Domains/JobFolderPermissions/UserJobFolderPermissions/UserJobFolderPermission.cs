using Core.Domains._Shared.Entities;
using Core.Domains.JobFolders;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobFolderPermissions.UserJobFolderPermissions;

public class UserJobFolderPermission : BaseEntity
{
    public static UserJobFolderPermissionValidator Validator { get; } = new();

    public static Result<UserJobFolderPermission> Create(long userId, long folderId, long folderPermissionId)
    {
        var uffPermission = new UserJobFolderPermission(userId, folderId, folderPermissionId);

        var validationResult = Validator.Validate(uffPermission);

        return validationResult.IsValid
            ? uffPermission
            : Result<UserJobFolderPermission>.Invalid(validationResult.AsErrors());
    }
    
    private UserJobFolderPermission(long userId, long folderId, long permissionId)
    {
        UserId = userId;
        FolderId = folderId;
        PermissionId = permissionId;
    }
    
    public long UserId { get; private set; }
    public long FolderId { get; private set; }
    public long PermissionId { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual JobFolder? Folder { get; set; }
    public virtual JobFolderPermission? FolderPermission { get; set; }
}