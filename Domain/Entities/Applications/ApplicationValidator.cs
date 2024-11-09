using FluentValidation;

namespace Domain.Entities.Applications;

public class ApplicationValidator : AbstractValidator<Application>
{
    public ApplicationValidator()
    {
        RuleFor(x => x.JobId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
    }
}