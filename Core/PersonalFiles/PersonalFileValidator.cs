using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.PersonalFiles;

public class PersonalFileValidator : AbstractValidator<PersonalFile>
{
    public PersonalFileValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Extension).Length(1, 10)
            .WhitelistPolicy(new WhitelistPolicy().StandardLatinLetters().CustomChars(['_']));

        RuleFor(x => x.Size).InclusiveBetween(1, 50000000);
    }
}