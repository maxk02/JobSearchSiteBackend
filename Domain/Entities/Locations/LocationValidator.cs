using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.Locations;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.ParentId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Level).InclusiveBetween(1, 15);
        
        RuleFor(x => x.Name).Length(1, 70)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Description).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Code).Length(1, 20)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols());
    }
}