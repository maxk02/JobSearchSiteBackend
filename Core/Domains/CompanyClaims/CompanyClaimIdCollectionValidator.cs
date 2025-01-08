using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.CompanyClaims;

public class CompanyClaimIdCollectionValidator : AbstractValidator<ICollection<long>>
{
    public CompanyClaimIdCollectionValidator()
    {
        RuleFor(x => x).MustAllExistIn(CompanyClaim.AllIds);
        When(x => x.Contains(CompanyClaim.IsOwner.Id), () =>
        {
            RuleFor(x => x).Must(x => CompanyClaim.AllIds.All(x.Contains));
        });
        When(x => x.Contains(CompanyClaim.CanEditStats.Id), () =>
        {
            RuleFor(x => x).Must(x => x.Contains(CompanyClaim.CanReadStats.Id));
        });
    }
}