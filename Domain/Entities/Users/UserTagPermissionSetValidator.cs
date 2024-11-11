using FluentValidation;

namespace Domain.Entities.Users;

public class UserTagPermissionSetValidator : AbstractValidator<UserTagPermissionSet>
{
    public UserTagPermissionSetValidator()
    {
        RuleFor(x => x.TagId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);

        RuleFor(x => x.CanManageApplications).Equal(true)
            .When(x => x.IsTagAdmin || x.IsTagOwner);
        
        RuleFor(x => x.CanCreateEditDeleteJobs).Equal(true)
            .When(x => x.IsTagAdmin || x.IsTagOwner);
        
        RuleFor(x => x.IsTagAdmin).Equal(true)
            .When(x => x.IsTagOwner);
    }
}