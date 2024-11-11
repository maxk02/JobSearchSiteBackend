using FluentValidation;

namespace Domain.Entities.Users;

public class UserCompanyBookmarkValidator : AbstractValidator<UserCompanyBookmark>
{
    public UserCompanyBookmarkValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
    }
}