using Domain.JSONEntities;
using FluentValidation;

namespace Application.Common.Validators;

public class AddressRecordValidator : AbstractValidator<AddressRecord?>
{
    public AddressRecordValidator()
    {
        RuleFor(x => x!.Line1).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Line2).MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Description).MaximumLength(100).When(x => x != null);
    }
}