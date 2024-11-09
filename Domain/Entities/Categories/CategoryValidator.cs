using FluentValidation;

namespace Domain.Entities.Categories;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.ParentId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Level).InclusiveBetween(0, 15);
    }
}