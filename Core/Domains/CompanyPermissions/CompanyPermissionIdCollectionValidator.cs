using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.CompanyPermissions;

public class CompanyPermissionIdCollectionValidator : AbstractValidator<ICollection<long>>
{
    public CompanyPermissionIdCollectionValidator()
    {
        RuleFor(x => x).MustAllExistIn(CompanyPermission.AllIds);
        When(x => x.Contains(CompanyPermission.IsOwner.Id), () =>
        {
            RuleFor(x => x).Must(x => CompanyPermission.AllIds.All(x.Contains));
        });
        When(x => x.Contains(CompanyPermission.CanEditStats.Id), () =>
        {
            RuleFor(x => x).Must(x => x.Contains(CompanyPermission.CanReadStats.Id));
        });
    }
}