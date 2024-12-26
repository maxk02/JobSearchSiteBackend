using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.JobFolderPermissions;

public class JobFolderPermissionIdCollectionValidator : AbstractValidator<ICollection<long>>
{
    public JobFolderPermissionIdCollectionValidator()
    {
        RuleFor(x => x).MustAllExistIn(JobFolderPermission.AllIds);
        When(x => x.Contains(JobFolderPermission.IsOwner.Id), () =>
        {
            RuleFor(x => x).Must(x => JobFolderPermission.AllIds.All(x.Contains));
        });
        When(x => x.Contains(JobFolderPermission.CanEditStats.Id), () =>
        {
            RuleFor(x => x).Must(x => x.Contains(JobFolderPermission.CanReadStats.Id));
        });
    }
}