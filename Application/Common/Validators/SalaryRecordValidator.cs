using Domain.ValueObjects;
using FluentValidation;

namespace Application.Common.Validators;

public class SalaryRecordValidator : AbstractValidator<SalaryRecord?>
{
    public SalaryRecordValidator()
    {
        RuleFor(x => x!.Minimum).GreaterThanOrEqualTo(0).When(x => x != null && x.Minimum != null);
        RuleFor(x => x!.Maximum).GreaterThanOrEqualTo(0).When(x => x != null && x.Maximum != null);
        
        // RuleFor(x => x!.CurrencyValue).NotNull().IsInEnum()
        //     .When(
        //         x => x != null && (
        //             (x.Minimum != null && x.Minimum > 0) || (x.Maximum != null && x.Maximum > 0)
        //         )
        //     );
        
        RuleFor(x => x!.TimeValue).NotNull().IsInEnum()
            .When(
                x => x != null && (
                    (x.Minimum != null && x.Minimum > 0) || (x.Maximum != null && x.Maximum > 0)
                )
            );
    }
}