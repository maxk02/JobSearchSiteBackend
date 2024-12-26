using FluentValidation;

namespace Core.Domains.CompanyPermissions.UserCompanyPermissions;

public class UserCompanyPermissionValidator : AbstractValidator<UserCompanyPermission>
{
    public UserCompanyPermissionValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PermissionId).GreaterThanOrEqualTo(1);
    }
}