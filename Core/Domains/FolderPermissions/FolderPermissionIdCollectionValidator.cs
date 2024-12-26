using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.FolderPermissions;

public class FolderPermissionIdCollectionValidator : AbstractValidator<ICollection<long>>
{
    public FolderPermissionIdCollectionValidator()
    {
        RuleFor(x => x).MustAllExistIn(FolderPermission.AllIds);
        When(x => x.Contains(FolderPermission.IsOwner.Id), () =>
        {
            RuleFor(x => x).Must(x => FolderPermission.AllIds.All(x.Contains));
        });
        When(x => x.Contains(FolderPermission.CanEditStats.Id), () =>
        {
            RuleFor(x => x).Must(x => x.Contains(FolderPermission.CanReadStats.Id));
        });
    }
}