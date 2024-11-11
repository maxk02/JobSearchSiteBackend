using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.Companies;

public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Description).Length(1, 500)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols().Newlines());
    }
}