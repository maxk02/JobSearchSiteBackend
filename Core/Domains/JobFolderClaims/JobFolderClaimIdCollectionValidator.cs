using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.JobFolderClaims;

public class JobFolderClaimIdCollectionValidator : AbstractValidator<ICollection<long>>
{
    public JobFolderClaimIdCollectionValidator()
    {
        RuleFor(x => x).MustAllExistIn(JobFolderClaim.AllIds);
        When(x => x.Contains(JobFolderClaim.IsOwner.Id), () =>
        {
            RuleFor(x => x).Must(x => JobFolderClaim.AllIds.All(x.Contains));
        });
        When(x => x.Contains(JobFolderClaim.CanEditStats.Id), () =>
        {
            RuleFor(x => x).Must(x => x.Contains(JobFolderClaim.CanReadStats.Id));
        });
    }
}