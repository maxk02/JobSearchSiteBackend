using FluentValidation;

namespace Domain.Entities.Users;

public class UserCompanyPermissionSetValidator : AbstractValidator<UserCompanyPermissionSet>
{
    public UserCompanyPermissionSetValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);

        RuleFor(x => x.CanCreateTags).Equal(true)
            .When(x => x.IsCompanyAdmin || x.IsCompanyOwner);
        
        RuleFor(x => x.IsCompanyAdmin).Equal(true)
            .When(x => x.IsCompanyOwner);
    }
}