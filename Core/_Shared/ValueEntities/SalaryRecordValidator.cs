using Core._Shared.Enums;
using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core._Shared.ValueEntities;

public class SalaryRecordValidator : AbstractValidator<SalaryRecord>
{
    public SalaryRecordValidator()
    {
        RuleFor(x => x.Minimum).InclusiveBetween(new decimal(0.0), new decimal(1000000))
            .LessThanOrEqualTo(x => x.Maximum);
        
        RuleFor(x => x.Maximum).InclusiveBetween(new decimal(0.0), new decimal(1000000))
            .GreaterThanOrEqualTo(x => x.Minimum);

        RuleFor(x => x).Must(x => x.Minimum != null || x.Maximum != null)
            .WithMessage("Minimum or maximum wage must be specified.");

        RuleFor(x => x.CurrencyCode).MustExistIn(["PLN", "USD", "EUR"]);

        RuleFor(x => x.UnitOfTime).IsEnumName(typeof(UnitsOfTime));
    }
}