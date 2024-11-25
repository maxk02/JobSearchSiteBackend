using FluentValidation;

namespace Domain.Entities.UserFolderFolderPermissions;

public class UserFolderFolderPermissionValidator : AbstractValidator<UserFolderFolderPermission>
{
    public UserFolderFolderPermissionValidator()
    {
        RuleFor(x => x.FolderId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PermissionId).GreaterThanOrEqualTo(1);
    }
}