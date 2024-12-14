using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.Folders;

public class FolderValidator : AbstractValidator<Folder>
{
    public FolderValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.ParentId).GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Description).Length(1, 300)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols().Newlines());
    }
}