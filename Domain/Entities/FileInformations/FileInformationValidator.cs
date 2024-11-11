using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.FileInformations;

public class FileInformationValidator : AbstractValidator<FileInformation>
{
    public FileInformationValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Extension).Length(1, 10)
            .WhitelistPolicy(new WhitelistPolicy().StandardLatinLetters().CustomChars(['_']));

        RuleFor(x => x.SizeInBytes).InclusiveBetween(1, 50000000);
    }
}