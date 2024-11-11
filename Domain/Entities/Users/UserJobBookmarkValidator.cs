using FluentValidation;

namespace Domain.Entities.Users;

public class UserJobBookmarkValidator : AbstractValidator<UserJobBookmark>
{
    public UserJobBookmarkValidator()
    {
        RuleFor(x => x.JobId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
    }
}