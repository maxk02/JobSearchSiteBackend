using FluentValidation;

namespace Core.Domains.JobFolderPermissions.UserJobFolderPermissions;

public class UserJobFolderPermissionValidator : AbstractValidator<UserJobFolderPermission>
{
    public UserJobFolderPermissionValidator()
    {
        RuleFor(x => x.FolderId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PermissionId).GreaterThanOrEqualTo(1);
    }
}