using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.Categories;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.ParentId).GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.Level).InclusiveBetween(1, 15);
        
        RuleFor(x => x.Name).Length(1, 30)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
    }
}