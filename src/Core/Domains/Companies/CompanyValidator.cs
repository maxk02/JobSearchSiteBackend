using FluentValidation;
using JobSearchSiteBackend.Shared.FluentValidationAddons;
using JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

namespace Core.Domains.Companies;

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