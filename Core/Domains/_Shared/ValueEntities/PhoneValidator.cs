using FluentValidation;

namespace Core.Domains._Shared.ValueEntities;

public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        RuleFor(x => x.CountryCode).Length(1, 5).Matches(@"\d+").WithMessage("Only numbers are allowed here.");
        
        RuleFor(x => x.OperatorOrAreaCode).Length(1, 5).Matches(@"\d+").WithMessage("Only numbers are allowed here.");
        
        RuleFor(x => x.Number).Length(1, 15).Matches(@"\d+").WithMessage("Only numbers are allowed here.");
    }
}