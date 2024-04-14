using Domain.JSONEntities;
using FluentValidation;

namespace Application.Common.Validators;

public class EmploymentTypeRecordValidator : AbstractValidator<EmploymentTypeRecord?>
{
    public EmploymentTypeRecordValidator()
    {
        RuleFor(x => x!).Must(x => 
            x.IsHybrid.HasValue || 
            x.IsMobile.HasValue || 
            x.IsPartTime.HasValue || 
            x.IsFullTime.HasValue || 
            x.IsRemote.HasValue || 
            x.IsOnSite.HasValue
        ).When(x => x != null);
    }
}