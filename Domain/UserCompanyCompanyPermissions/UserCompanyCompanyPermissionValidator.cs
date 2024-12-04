using FluentValidation;

namespace Domain.UserCompanyCompanyPermissions;

public class UserCompanyCompanyPermissionValidator : AbstractValidator<UserCompanyCompanyPermission>
{
    public UserCompanyCompanyPermissionValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PermissionId).GreaterThanOrEqualTo(1);
    }
}