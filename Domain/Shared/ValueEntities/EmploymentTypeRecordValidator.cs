using FluentValidation;

namespace Domain.Shared.ValueEntities;

public class EmploymentTypeRecordValidator : AbstractValidator<EmploymentTypeRecord>
{
    public EmploymentTypeRecordValidator()
    {
        RuleFor(x => x)
            .Must(x => x.IsPartTime || x.IsFullTime || x.IsHybrid || x.IsMobile || x.IsRemote || x.IsOnSite);
    }
}