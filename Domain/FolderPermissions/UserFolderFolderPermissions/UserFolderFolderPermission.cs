using Domain._Shared.Entities;
using Domain.Folders;
using Domain.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.FolderPermissions.UserFolderFolderPermissions;

public class UserFolderFolderPermission : BaseEntity
{
    public static UserFolderFolderPermissionValidator Validator { get; } = new();

    public static Result<UserFolderFolderPermission> Create(long userId, long folderId, long folderPermissionId)
    {
        var uffPermission = new UserFolderFolderPermission(userId, folderId, folderPermissionId);

        var validationResult = Validator.Validate(uffPermission);

        return validationResult.IsValid
            ? uffPermission
            : Result<UserFolderFolderPermission>.Invalid(validationResult.AsErrors());
    }
    
    private UserFolderFolderPermission(long userId, long folderId, long permissionId)
    {
        UserId = userId;
        FolderId = folderId;
        PermissionId = permissionId;
    }
    
    public long UserId { get; private set; }
    public long FolderId { get; private set; }
    public long PermissionId { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual Folder? Folder { get; set; }
    public virtual FolderPermission? FolderPermission { get; set; }
}