using FluentValidation;
using JobSearchSiteBackend.Shared.FluentValidationAddons;
using JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles;

public class PersonalFileValidator : AbstractValidator<PersonalFile>
{
    public PersonalFileValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Extension).Length(1, 10)
            .WhitelistPolicy(new WhitelistPolicy().StandardLatinLetters().CustomChars(['_']))
            .MustExistIn(["pdf", "docx", "doc", "odt"]);

        RuleFor(x => x.Size).InclusiveBetween(1, 50000000);
    }
}