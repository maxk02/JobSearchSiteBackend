using FluentValidation;

namespace Domain._Shared.ValueEntities;

public class EmploymentTypeRecordValidator : AbstractValidator<EmploymentTypeRecord>
{
    public EmploymentTypeRecordValidator()
    {
        RuleFor(x => x)
            .Must(x => x.IsPartTime || x.IsFullTime || x.IsHybrid || x.IsMobile || x.IsRemote || x.IsOnSite)
            .WithMessage("This field either must be null or have at least one of employment types set to true.");
    }
}